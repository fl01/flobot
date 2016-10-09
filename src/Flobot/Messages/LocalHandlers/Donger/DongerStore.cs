using System.Collections.Generic;

namespace Flobot.Messages.LocalHandlers.Donger
{
    public class DongerStore
    {
        private List<DongerFace> dongerList;

        public DongerStore()
        {
            dongerList = new List<DongerFace>()
            {
                new DongerFace() { IsPopular = true, Text = "( ͡° ͜ʖ ͡°)" },
                new DongerFace() { IsPopular = true, Text = @"¯\＿(ツ)＿/¯" },
                new DongerFace() { IsPopular = true, Text = "(ง'̀-'́)ง" },
                new DongerFace() { IsPopular = true, Text = "( ᵔ◡ᵔ)" },
                new DongerFace() { IsPopular = true, Text = "ლ(ಠ益ಠლ)" },
                new DongerFace() { Text = "⊂(▀¯▀⊂)" },
                new DongerFace() { Text = "ԅ( ͒ ۝ ͒ )ᕤ" },
                new DongerFace() { Text = "٩ʕ◕౪◕ʔو" },
                new DongerFace() { Text = "(つ°ヮ°)つ  └⋃┘" },
                new DongerFace() { Text = "໒( ͡ᵔ ▾ ͡ᵔ )७" },
                new DongerFace() { Text = "໒( • ͜ʖ • )७" },
                new DongerFace() { Text = "/╲/╭〳 □ ʖ̫ □ 〵╮/╱﻿" },
                new DongerFace() { Text = "/╲/╭(˵◕╭ ͟ʖ╮◕˵)╮/╱﻿" },
                new DongerFace() { Text = "━╤デ╦︻(▀̿̿Ĺ̯̿̿▀̿ ̿)" },
                new DongerFace() { Text = "~~~~~~~[]=¤ԅ(ˊᗜˋ* )੭" },
                new DongerFace() { Text = "ヽ║ ˘ _ ˘ ║ノ" },
                new DongerFace() { Text = "| ” ☯ ︿ ☯ ” |" },
                new DongerFace() { Text = "┌∩┐╭˵ಥ o ಥ˵╮┌∩┐" },
                new DongerFace() { Text = "། ⊙ 益 ⊙ །" },
                new DongerFace() { Text = "╭∩╮( ͡°_ل͟ ͡° )╭∩╮" }
            };
        }

        public IReadOnlyCollection<DongerFace> GetAllDongers()
        {
            return dongerList.AsReadOnly();
        }
    }
}
