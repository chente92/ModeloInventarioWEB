using NumerosAleatorios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NumerosAleatorios.Controllers
{
    public class InventarioController : Controller
    {
        EjercicioMontecarlo ejercicio = EjercicioMontecarlo.Instance;
        //
        // GET: /Inventario/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Ingreso() 
        {
            return View();
        }

        public ActionResult Aleatorios() 
        {
            

            ejercicio.puntoReordenar = Convert.ToDecimal(Request.Form["puntoreorden"]);
            ejercicio.inventarioExistenciaInicial = Convert.ToDecimal(Request.Form["inventarioinicial"]);
            ejercicio.cantidadPorPedido = Convert.ToDecimal(Request.Form["cantidadpedido"]);
            ejercicio.cantidadNumerosAleatorios = Convert.ToInt32(Request.Form["numerostabla"]);


            List<decimal> demanda = new List<decimal>();
            demanda.Add(Convert.ToDecimal(Request.Form["demanda1"]));
            demanda.Add(Convert.ToDecimal(Request.Form["demanda2"]));
            demanda.Add(Convert.ToDecimal(Request.Form["demanda3"]));
            demanda.Add(Convert.ToDecimal(Request.Form["demanda4"]));
            demanda.Add(Convert.ToDecimal(Request.Form["demanda5"]));
            ejercicio.demanda = demanda;

            List<decimal> retraso = new List<decimal>();
            retraso.Add(Convert.ToDecimal(Request.Form["retraso1"]));
            retraso.Add(Convert.ToDecimal(Request.Form["retraso2"]));
            retraso.Add(Convert.ToDecimal(Request.Form["retraso3"]));
            retraso.Add(Convert.ToDecimal(Request.Form["retraso4"]));
            retraso.Add(Convert.ToDecimal(Request.Form["retraso5"]));
            ejercicio.retraso = retraso;

            
            List<decimal> distribucionR = new List<decimal>();
            distribucionR.Add(Convert.ToDecimal(Request.Form["drelativa1"]));
            distribucionR.Add(Convert.ToDecimal(Request.Form["drelativa2"]));
            distribucionR.Add(Convert.ToDecimal(Request.Form["drelativa3"]));
            distribucionR.Add(Convert.ToDecimal(Request.Form["drelativa4"]));
            distribucionR.Add(Convert.ToDecimal(Request.Form["drelativa5"]));
            ejercicio.distribucionRelativa = distribucionR;

            List<decimal> distribucionA = new List<decimal>();
            distribucionA.Add(Convert.ToDecimal(Request.Form["dacomulada1"]));
            distribucionA.Add(Convert.ToDecimal(Request.Form["dacomulada2"]));
            distribucionA.Add(Convert.ToDecimal(Request.Form["dacomulada3"]));
            distribucionA.Add(Convert.ToDecimal(Request.Form["dacomulada4"]));
            distribucionA.Add(Convert.ToDecimal(Request.Form["dacomulada5"]));
            ejercicio.distribucionAcomulada = distribucionA;

            List<decimal> retrasoR = new List<decimal>();
            retrasoR.Add(Convert.ToDecimal(Request.Form["rrelativa1"]));
            retrasoR.Add(Convert.ToDecimal(Request.Form["rrelativa2"]));
            retrasoR.Add(Convert.ToDecimal(Request.Form["rrelativa3"]));
            retrasoR.Add(Convert.ToDecimal(Request.Form["rrelativa4"]));
            retrasoR.Add(Convert.ToDecimal(Request.Form["rrelativa5"]));
            ejercicio.retrasoRelativo = retrasoR;
  
            List<decimal> retrasoA = new List<decimal>();
            retrasoA.Add(Convert.ToDecimal(Request.Form["racomulada1"]));
            retrasoA.Add(Convert.ToDecimal(Request.Form["racomulada2"]));
            retrasoA.Add(Convert.ToDecimal(Request.Form["racomulada3"]));
            retrasoA.Add(Convert.ToDecimal(Request.Form["racomulada4"]));
            retrasoA.Add(Convert.ToDecimal(Request.Form["racomulada5"]));
            ejercicio.retrasoAcomulado = retrasoA;

            ViewBag.ejercicio = ejercicio;
            
            return View();
        }

        public ActionResult Tabla()
        {
            
            ViewBag.cantidadNumerosAleatoreos = ejercicio.puntoReordenar;

            List<decimal> aleatoriosDistribucion = new List<decimal>();
            List<decimal> aleatoriosRetraso = new List<decimal>();
            for (int i = 0; i < Request.Form.Count; i++)
			{
                string nameInput = Request.Form.GetKey(i);

                if (nameInput.StartsWith("distribucion_")) 
                {
                    aleatoriosDistribucion.Add(Convert.ToDecimal(Request.Form[nameInput])) ;
                }
                if (nameInput.StartsWith("retraso_"))
                {
                    aleatoriosRetraso.Add(Convert.ToDecimal(Request.Form[nameInput]));
                }

                ejercicio.aleatoriosDistribucion = aleatoriosDistribucion;
                ejercicio.aleatoriosRetraso = aleatoriosRetraso;
			}

            

            ejercicio.calcularModelo();
            ViewBag.ejercicio = ejercicio;

            return View();
        }
	}
}