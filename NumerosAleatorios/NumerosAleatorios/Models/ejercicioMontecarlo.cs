using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NumerosAleatorios.Models
{
    public class EjercicioMontecarlo
    {
    private static EjercicioMontecarlo instance = null;
    private static readonly object padlock = new object();

    EjercicioMontecarlo()
    {
    }

    public static EjercicioMontecarlo Instance
    {
        get
        {
            lock (padlock)
            {
                if (HttpContext.Current.Session["Instance"] == null)
                {
                    HttpContext.Current.Session["Instance"]
                    = instance = new EjercicioMontecarlo();
                }
            }
            return (EjercicioMontecarlo)HttpContext.Current.Session["Instance"];
            
        }
    }
    
       
    
   

         public decimal cantidadPorPedido { get; set; }
         public decimal inventarioExistenciaInicial { get; set; }
         public decimal puntoReordenar { get; set; }
         public int cantidadNumerosAleatorios { get; set; }
         public List<decimal> distribucionRelativa { get; set; }
         public List<decimal> distribucionAcomulada { get; set; }
         public List<decimal> retrasoRelativo { get; set; }
         public List<decimal> retrasoAcomulado { get; set; }
         public List<decimal> aleatoriosDistribucion { get; set; }
         public List<decimal> aleatoriosRetraso { get; set; }
         public List<decimal> demanda { get; set; }
         public List<decimal> retraso { get; set; }

         public TablaResultado tablaResultado { get; set; }

         public TablaResultado calcularModelo() 
         {
             tablaResultado = new TablaResultado();
             tablaResultado.filas = new List<FilaIteracion>();
             decimal inventario = this.inventarioExistenciaInicial;
             decimal retrasoPedido = 0;
             int indexRetraso = 0;
             bool llegoPedido = false;
             bool enEsperaPedido = false;
           
             for (int i = 0; i < cantidadNumerosAleatorios; i++)
             {
                 FilaIteracion iteracion = new FilaIteracion();
                 iteracion.iteracionNumero = (i+1);
                 iteracion.numeroAleatorioDemanda = this.aleatoriosDistribucion[i];
                 //Comprobacion si llego el pedido
                 if (llegoPedido == true)
                 {
                     inventario += this.cantidadPorPedido;//Agrego el pedido que llego al inventario
                     llegoPedido = false;
                     enEsperaPedido = false;

                 }


                 //Venta de unidades
                 iteracion.ventasUnidades = encontrarRango(iteracion.numeroAleatorioDemanda,TipoDistribucion.demanda);

                 //Inventario
                 iteracion.inventarioInicial = inventario;
                 if ((inventario - iteracion.ventasUnidades) <= 0)
                 {
                     iteracion.inventraioFinal = 0;
                     iteracion.ventasPerdidasEscasez = Math.Abs(inventario - iteracion.ventasUnidades);
                     
                 }
                 else
                 { 
                    iteracion.inventraioFinal = inventario - iteracion.ventasUnidades;
                 }
                 
                 //Retraso del pedido
                 retrasoPedido--;
                 if (retrasoPedido == 0)
                 {
                     iteracion.recibosBodega = this.cantidadPorPedido;
                     llegoPedido = true;
                     

                 }
                 else
                 {
                     iteracion.recibosBodega = 0;
                 }



                 if ((inventario) <= this.puntoReordenar && retrasoPedido <= 0 && !enEsperaPedido)
                 {
                     iteracion.colocarPedido = "SI";
                     iteracion.numeroAleatorioEntrega = this.aleatoriosRetraso[indexRetraso];
                     iteracion.retrasoPedido = encontrarRango(iteracion.numeroAleatorioEntrega, TipoDistribucion.retraso);                    
                     retrasoPedido = iteracion.retrasoPedido;
                     indexRetraso++;
                     enEsperaPedido = true;
                 }
                 else 
                 {
                     iteracion.colocarPedido = "NO";
                     iteracion.numeroAleatorioEntrega = 0;
                     iteracion.retrasoPedido = 0;
                 }

                 

                 

                 iteracion.numeroAleatorioDemanda = this.aleatoriosDistribucion[i];
                 if (iteracion.inventraioFinal < this.puntoReordenar)
                 {
                     iteracion.cantidadPedidoEnEspera = this.cantidadPorPedido;
                 }
                 else 
                 {
                     iteracion.cantidadPedidoEnEspera = 0;
                 }

                 if (iteracion.colocarPedido == "SI") 
                 {
                     iteracion.cantidadPedidoSolicitado = this.cantidadPorPedido;
                 }
                 else
                 {
                     iteracion.cantidadPedidoSolicitado = 0;
                 }

                 if (iteracion.inventraioFinal < 0)
                 {
                     iteracion.faltantesVentasPerdida = (iteracion.inventraioFinal * -1) + iteracion.inventarioInicial;
                 }
                 else 
                 {
                     iteracion.faltantesVentasPerdida = 0;
                 }

                 //actualizacion de variable inventario
                 inventario = iteracion.inventraioFinal;

                 tablaResultado.filas.Add(iteracion);
                 
                 
             }
             return tablaResultado;
         }

        enum TipoDistribucion
        {
            demanda,
            retraso
        }
         decimal encontrarRango(decimal aleatorio, TipoDistribucion tipoDistribucion) 
         {
             int demanda = 0;

             List<decimal> distribucion = new List<decimal>();
             List<decimal> valores = new List<decimal>();


             if (tipoDistribucion == TipoDistribucion.demanda) 
             {
                 distribucion = this.distribucionAcomulada;
                 valores = this.demanda;
             }
             else if (tipoDistribucion == TipoDistribucion.retraso)
             {
                 distribucion = this.retrasoAcomulado;
                 valores = this.retraso;
             }

             foreach (decimal item in distribucion)
             {
                
                 if (aleatorio <= item) 
                 {
                     return Convert.ToDecimal(valores[demanda]);
                     
                 }
                 demanda++;
                 
             }
             return 0;
         }


    }

    public class TablaResultado 
    {
        public List<string> cabeceras { get; set; }
        public List<FilaIteracion> filas { get; set; }
        
    }

    public class FilaIteracion 
    {
        public int iteracionNumero { get; set; }
        public decimal numeroAleatorioDemanda { get; set; }
        public decimal numeroAleatorioEntrega { get; set; }
        public decimal ventasUnidades { get; set; }
        public decimal retrasoPedido { get; set; }
        public decimal recibosBodega { get; set; }
        public decimal cantidadPedidoSolicitado { get; set; }
        public decimal inventarioInicial { get; set; }
        public decimal inventraioFinal { get; set; }
        public decimal faltantesVentasPerdida { get; set; }
        public decimal cantidadPedidoEnEspera { get; set; }
        public decimal despachoPedidoSemanal { get; set; }
        public decimal ventasPerdidasEscasez { get; set; }
        public string colocarPedido { get; set; }
    }
}