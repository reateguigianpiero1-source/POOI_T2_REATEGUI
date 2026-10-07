using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using POOI_T2_REATEGUI.Models;
using System.IO;

namespace POOI_T2_REATEGUI.Controllers
{
    public class AlumnoController : Controller
    {
        private static string lista = @"[]";

        private List<Alumno> ObtenerAlumnos()
        {
            return JsonConvert.DeserializeObject<List<Alumno>>(lista);
        }

        private void GuardarAlumnos(List<Alumno> alumnos)
        {
            lista = JsonConvert.SerializeObject(alumnos);
        }

        private Alumno Buscar(string dni)
        {
            return ObtenerAlumnos().FirstOrDefault(a => a.Dni == dni);
        }

        public ActionResult principal()
        {
            return View(ObtenerAlumnos());
        }

        [HttpGet]
        public ActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(string dni, string nombres, string apellidos, string carrera, int ciclo)
        {
            try
            {
                var alumnos = ObtenerAlumnos();
                if (alumnos.Any(a => a.Dni == dni))
                {
                    ViewBag.Error = "El DNI ya existe";
                    return View();
                }
                alumnos.Add(new Alumno(dni, nombres, apellidos, carrera, ciclo));
                GuardarAlumnos(alumnos);
                return RedirectToAction("principal");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public ActionResult Eliminar(string dni)
        {
            try
            {
                var alumnos = ObtenerAlumnos();
                alumnos.RemoveAll(a => a.Dni == dni);
                GuardarAlumnos(alumnos);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("principal");
        }

        public ActionResult Detalles(string dni)
        {
            try
            {
                var alumno = Buscar(dni);
                if (alumno == null)
                {
                    return RedirectToAction("principal");
                }
                return View(alumno);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("principal");
            }
        }

        [HttpGet]
        public ActionResult Actualizar(string dni)
        {
            try
            {
                var alumno = Buscar(dni);
                if (alumno == null)
                {
                    return RedirectToAction("principal");
                }
                ViewBag.DniOriginal = dni;
                return View(alumno);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("principal");
            }
        }

        [HttpPost]
        public ActionResult Actualizar(string dniOriginal, string dni, string nombres, string apellidos, string carrera, int ciclo)
        {
            try
            {
                var alumnos = ObtenerAlumnos();
                if (alumnos.Any(a => a.Dni == dni && a.Dni != dniOriginal))
                {
                    ViewBag.Error = "El DNI ya existe";
                    ViewBag.DniOriginal = dniOriginal;
                    return View(new Alumno(dni, nombres, apellidos, carrera, ciclo));
                }
                int posicion = alumnos.FindIndex(a => a.Dni == dniOriginal);
                alumnos[posicion] = new Alumno(dni, nombres, apellidos, carrera, ciclo);
                GuardarAlumnos(alumnos);
                return RedirectToAction("principal");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("principal");
            }
        }
    }
}