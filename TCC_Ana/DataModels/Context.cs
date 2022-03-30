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

        public DbSet<CaixaAguaDb> CatalogoCaixas { get; set; }
        public DbSet<EndDeviceDb> EndDevices { get; set; }
        //public DbSet<UserDevices> EndDevices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //string connectiontring = _configuration.GetValue<string>("kv-connectionstring-db-tcc-ana");

            //optionsBuilder.UseSqlServer(connectiontring);
            //optionsBuilder.UseSqlServer("Data Source=mssqlaconnect.itmnetworks.net;Initial Catalog=1307_tcc_ana;Persist Security Info=True;User ID=1307_leonaldo;Password=Uq4amivb[w");
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-L5IFHFJ;Initial Catalog=tcc-ana;Integrated Security=True");
            
        }

    }
}
