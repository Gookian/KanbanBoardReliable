using Core;
using Kanban.DesktopClient.Models;
using Kanban.DesktopClient.RestAPI;
using Kanban.DesktopClient.Views;
using Prism.Commands;
using System;

namespace Kanban.DesktopClient.ViewModels
{
    public class KanbanPageViewModel
    {
        public DelegateCommand CreateColumn { get; set; }
        public DelegateCommand PreviousPage { get; set; }

        public KanbanPageViewModel()
        {
            CreateColumn = new DelegateCommand(CreateColumn_Click);
            PreviousPage = new DelegateCommand(PreviousPage_Click);

            /*Timer timer = new Timer(timerTick);
            timer.Start();*/
        }
/*
        private async void timerTick(object sender, EventArgs e)
        {
            if (Context.targetPage != null)
            {
                if (Context.targetPage.GetType() == typeof(KanbanPage))
                {
                    UIFactory.UppdateDataBoard();
                }
                else if (Context.targetPage.GetType() == typeof(BoardPage))
                {
                    UIFactory.UppdateDataBoards();
                }
            }
        }*/

        public async void CreateColumn_Click()
        {
            if (BindingContext.PlaceToPupup.Children.Count < 1)
                BindingContext.PlaceToPupup.Children.Add(UIFactory.CreatePupupColumn(new Column(), "Add"));
        }

        public void PreviousPage_Click()
        {
            BindingContext.MainFrame.Child = BindingContext.HomePage;
            BindingContext.HomeFrame.Child = BindingContext.BoardPage;
        }
    }
}
