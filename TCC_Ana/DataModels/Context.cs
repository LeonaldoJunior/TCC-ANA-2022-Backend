using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TCC_Ana.DataModels
{
    public class Context: DbContext
    {
        readonly IConfiguration _configuration;

        public Context(IConfiguration iConfig)
        {
            _configuration = iConfig;
        }

        public DbSet<EventsEndDevice> EventsEndDevices { get; set; }
        public DbSet<WaterTankList> WaterTankList { get; set; }
        public DbSet<EndDeviceList> EndDeviceList { get; set; }
        public DbSet<UserList> UserList { get; set; }
        public DbSet<UsersAndDevices> UsersAndDevices { get; set; }
        public DbSet<VolumeCalculation> VolumeCalculations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //string connectiontring = _configuration.GetValue<string>("kv-connectionstring-db-tcc-ana");

            //optionsBuilder.UseSqlServer(connectiontring);
            optionsBuilder.UseSqlServer("Data Source=mssqlaconnect.itmnetworks.net;Initial Catalog=1307_tcc_ana;Persist Security Info=True;User ID=1307_leonaldo;Password=Uq4amivb[w");
            //optionsBuilder.UseSqlServer("Data Source=DESKTOP-L5IFHFJ;Initial Catalog=tcc-ana;Integrated Security=True");

        }

    }
}
