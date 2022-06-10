using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Kanban.DesktopClient.Models
{
    public class StackPanelRepository
    {
        private static List<EntityRepository> entityRepositories = new List<EntityRepository>();

        public static void Add(StackPanel stackPanel, Guid id)
        {
            entityRepositories.Add(new EntityRepository()
            {
                StackPanel = stackPanel,
                Id = id
            });
        }

        public static StackPanel GetById(Guid id)
        {
            foreach (var entityRepository in entityRepositories)
            {
                if (entityRepository.Id == id)
                    return entityRepository.StackPanel;
            }

            return null;
        }

        public static void Clear()
        {
            entityRepositories.Clear();
        }
    }

    public class EntityRepository
    {
        public StackPanel StackPanel { get; set; }
        public Guid Id { get; set; }
    }
}
