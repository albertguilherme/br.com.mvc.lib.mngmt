using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using br.com.mvc.lib.mngmt.model;
using Microsoft.EntityFrameworkCore;

namespace br.com.mvc.lib.mngmt.repository
{
    public partial class MNGMTContext
    {
        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            AddTimestamps();
            //ChangeDelete();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ChangeDelete()
        {
            ChangeTracker.DetectChanges();

            var markedAsDeleted = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);

            foreach (var item in markedAsDeleted)
            {
                if (item.Entity is not BaseModel entity) continue;
                item.State = EntityState.Unchanged;
                entity.DeleteAt = DateTime.Now;
            }
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseModel && (x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted));

            foreach (var entity in entities)
            {
                var now = DateTime.Now; // current datetime

                switch (entity.State)
                {
                    case EntityState.Added:
                        ((BaseModel)entity.Entity).CreateAt = now;
                        break;
                    case EntityState.Deleted:
                    {
                        ((BaseModel) entity.Entity).DeleteAt = now;
                        ((BaseModel)entity.Entity).IsDeleted = true;

                        break;
                    }
                }

                ((BaseModel)entity.Entity).UpdateAt = now;
            }
        }
    }
}
