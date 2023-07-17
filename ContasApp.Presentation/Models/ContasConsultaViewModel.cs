using System.ComponentModel.DataAnnotations;

namespace ContasApp.Presentation.Models
{
    /// <summary>
    /// Modelo de dados para o formulário da página de consulta de contas
    /// </summary>
    public class ContasConsultaViewModel
    {
        [Required(ErrorMessage = "Por favor, informe a data de início.")]
        public DateTime? DataInicio { get; set; }

        [Required(ErrorMessage = "Por favor, informe a data de término.")]
        public DateTime? DataFim { get; set; }
    }
}
