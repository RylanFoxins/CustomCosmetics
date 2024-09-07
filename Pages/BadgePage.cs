﻿using BananaOS;
using BananaOS.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static CustomCosmetics.Extensions.InfoLoader;

namespace CustomCosmetics
{
    class BadgePage : WatchPage
    {
        public override string Title => "Badges";

        public override bool DisplayOnMainMenu => false;
        List<string> badges = new List<string>();
        int pageNum = 1;
        int currentSelected = 0;
        int maxCurrentPage;
        int minCurrentPage;
        double maxPages;

        public override void OnPostModSetup()
        {
            base.OnPostModSetup();
            string path = Plugin.instance.cosmeticPath + "/Badges";
            foreach (string file in Directory.GetFiles(path))
            {
                badges.Add(file);
            }
        }

        public override string OnGetScreenContent()
        {
            StringBuilder str = new StringBuilder();
            if(badges.Count <= 0)
            {
                str.AppendLine($"No {Title} installed");
                return str.ToString();
            }
            double mathstuff = Convert.ToDouble(badges.Count) / 9.0;
            maxPages = Math.Ceiling(mathstuff);
            str.AppendLine("<color=yellow>==</color> Badges <color=yellow>==</color>");
            if (pageNum == maxPages)
            {
                int t = 9 * pageNum - badges.Count;
                int ii = 0;
                minCurrentPage = 9 * pageNum - 9;
                for (int i = 9 * pageNum - 9; i < 9 * pageNum - t; i++)
                {
                    string hatName = Path.GetFileNameWithoutExtension(badges[i]);
                    str.AppendLine(selectionHandler.GetOriginalBananaOSSelectionText(ii, hatName));
                    ii++;
                    minCurrentPage++;
                }
                maxCurrentPage = minCurrentPage - 1;
                minCurrentPage = 9 * pageNum - 9;
                selectionHandler.maxIndex = ii - 1;
            }
            else
            {
                int ii = 0;
                int t = 9 * pageNum - 9;
                for (int i = 9 * pageNum - 9; i < 9 * pageNum; i++)
                {
                    string hatName = Path.GetFileNameWithoutExtension(badges[i]);
                    str.AppendLine(selectionHandler.GetOriginalBananaOSSelectionText(ii, hatName));
                    ii++;
                    minCurrentPage++;
                }
                maxCurrentPage = minCurrentPage - 1;
                minCurrentPage = 9 * pageNum - 9;
                selectionHandler.maxIndex = 8;
            }
            str.AppendLine($"{pageNum}/{maxPages}");
            return str.ToString();
        }
        public override void OnButtonPressed(WatchButtonType buttonType)
        {
            switch (buttonType)
            {
                case WatchButtonType.Up:
                    selectionHandler.MoveSelectionUp();
                    currentSelected--;
                    if (currentSelected < 0)
                    {
                        currentSelected = maxCurrentPage;
                    }
                    break;
                case WatchButtonType.Down:
                    selectionHandler.MoveSelectionDown();
                    currentSelected++;
                    if (currentSelected > maxCurrentPage)
                    {
                        currentSelected = minCurrentPage;
                    }
                    break;
                case WatchButtonType.Left:
                    pageNum--;
                    if (pageNum < 1)
                    {
                        pageNum = (int)maxPages;
                    }
                    OnGetScreenContent();
                    currentSelected = minCurrentPage;
                    selectionHandler.currentIndex = 0;
                    break;
                case WatchButtonType.Right:
                    pageNum++;
                    if (pageNum > maxPages)
                    {
                        pageNum = 1;
                    }
                    OnGetScreenContent();
                    currentSelected = minCurrentPage;
                    selectionHandler.currentIndex = 0;
                    break;
                case WatchButtonType.Enter:
                    GetInfo(Path.GetFileName(badges[currentSelected]), "Badge");
                    SwitchToPage(typeof(BadgeLoadPage));
                    break;
                case WatchButtonType.Back:
                    SwitchToPage(typeof(CosmeticPage));
                    break;
            }
        }
    }
}
