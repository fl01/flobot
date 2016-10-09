using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flobot.Handlers.DataSource;
using Flobot.Handlers.Metadata;
using Flobot.Logging;
using Flobot.Settings;

namespace Flobot.Handlers
{
    public class HandlerMetadataService : IHandlerMetadataService
    {
        private List<IHandlerMetadata> metadata;
        private List<IMetadataDataSource> dataSources;
        private readonly ISettingsService settingsService;
        private readonly object handlersUpdateLock = new object();
        private readonly ILoggingService loggingService;
        private CancellationTokenSource cancellationSource;
        private bool isInitialized = false;

        public HandlerMetadataService(ILoggingService loggingService, ISettingsService settingsService)
        {
            this.settingsService = settingsService;
            this.loggingService = loggingService;
            metadata = new List<IHandlerMetadata>();
            dataSources = new List<IMetadataDataSource>();
            cancellationSource = new CancellationTokenSource();
        }

        ~HandlerMetadataService()
        {
            cancellationSource.Dispose();
        }

        public IReadOnlyCollection<IHandlerMetadata> GetHandlersMetadata()
        {
            lock (handlersUpdateLock)
            {
                return metadata.AsReadOnly();
            }
        }

        public IHandlerMetadataService AddDataSource(IMetadataDataSource dataSource)
        {
            if (dataSource == null)
            {
                throw new ArgumentNullException(nameof(dataSource));
            }

            dataSources.Add(dataSource);
            return this;
        }

        public void EnsureInitialized()
        {
            TimeSpan updateFrequency = settingsService.GetUpdateHandlersFrequency();
            Task.Factory.StartNew(x => UpdateHandlersList(updateFrequency, cancellationSource.Token), TaskCreationOptions.LongRunning, cancellationSource.Token);

            WaitInitialization();
        }

        private async void WaitInitialization()
        {
            while (!isInitialized)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(100));
            }
        }

        private async void UpdateHandlersList(TimeSpan updateFrequency, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var freshMetadata = new List<IHandlerMetadata>();

                foreach (IMetadataDataSource dataSource in dataSources)
                {
                    token.ThrowIfCancellationRequested();

                    IEnumerable<IHandlerMetadata> sourceHandlers = dataSource.GetAll().ToList();
                    freshMetadata.AddRange(sourceHandlers);
                }

                lock (handlersUpdateLock)
                {
                    metadata = freshMetadata;
                }

                if (!isInitialized)
                {
                    isInitialized = true;
                }
            }

            await Task.Delay(updateFrequency);
        }
    }
}