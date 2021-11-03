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

        DbSet<EndDeviceDb> EndDevices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectiontring = _configuration.GetValue<string>("kv-connectionstring-db-tcc-ana");

            optionsBuilder.UseSqlServer(connectiontring);
        }
    }
}
