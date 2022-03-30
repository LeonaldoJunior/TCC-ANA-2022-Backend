using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCC_Ana.DataModels
{
    public class CaixaAguaDb
    {
        [Key]
        public int CaixaId { get; set; }
        public string Marca { get; set; }
        public decimal RaioBase { get; set; }
        public decimal RaioTopo { get; set; }
        public decimal Altura { get; set; }
        public int Capacidade { get; set; }

    }

}