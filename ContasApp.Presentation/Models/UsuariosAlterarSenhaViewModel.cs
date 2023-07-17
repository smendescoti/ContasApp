using System.ComponentModel.DataAnnotations;

namespace ContasApp.Presentation.Models
{
    /// <summary>
    /// Modelo de dados para a página de alteração de senha do usuário
    /// </summary>
    public class UsuariosAlterarSenhaViewModel
    {
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
            ErrorMessage = "Informe uma senha com letras maísculas, letras minúsculas, números, símbolos e no mínimo 8 caracteres.")]
        [Required(ErrorMessage = "Por favor, informe sua senha atual.")]
        public string? SenhaAtual { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
            ErrorMessage = "Informe uma senha com letras maísculas, letras minúsculas, números, símbolos e no mínimo 8 caracteres.")]
        [Required(ErrorMessage = "Por favor, informe a nova senha.")]
        public string? NovaSenha { get; set; }

        [Compare("NovaSenha", ErrorMessage = "Senhas não conferem.")]
        [Required(ErrorMessage = "Por favor, confirme a nova senha.")]
        public string? NovaSenhaConfirmacao { get; set; }
    }
}
