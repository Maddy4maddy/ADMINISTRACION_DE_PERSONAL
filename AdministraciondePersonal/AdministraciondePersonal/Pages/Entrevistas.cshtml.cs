using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdministraciondePersonal.Pages
{
    public class EntrevistasModel : PageModel
    {
        private readonly EntrevistaService _entrevistaService;

        public EntrevistasModel(EntrevistaService entrevistaService)
        {
            _entrevistaService = entrevistaService;
        }

        public List<Entrevista> ListaEntrevistas { get; set; } = new List<Entrevista>();

        public string Mensaje { get; set; }

        public string Error { get; set; }

        public int PaginaActual { get; set; }

        public int TotalPaginas { get; set; }

        public int TamanioPagina { get; set; } = 10;

        public void OnGet(int pagina = 1)
        {
            CargarDatos(pagina);
        }

        public void OnPostEliminar(int idEntrevista, int paginaActual = 1)
        {
            string resultado = _entrevistaService.Eliminar(idEntrevista);

            if (resultado == "La entrevista ha sido eliminada correctamente.")
                Mensaje = resultado;
            else
                Error = resultado;

            CargarDatos(paginaActual);
        }

        public void OnPostMarcarRealizada(int idEntrevista, int paginaActual = 1)
        {
            string resultado = _entrevistaService.MarcarComoRealizada(idEntrevista);

            if (resultado == "La entrevista ha sido marcada como realizada.")
                Mensaje = resultado;
            else
                Error = resultado;

            CargarDatos(paginaActual);
        }

        private void CargarDatos(int pagina)
        {
            if (pagina <= 0)
                pagina = 1;

            PaginaActual = pagina;

            int totalRegistros = _entrevistaService.ContarEntrevistas();

            TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)TamanioPagina);

            if (TotalPaginas == 0)
                TotalPaginas = 1;

            ListaEntrevistas = _entrevistaService.ObtenerPaginado(PaginaActual, TamanioPagina);
        }
    }
}