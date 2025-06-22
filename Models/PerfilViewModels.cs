using System.ComponentModel.DataAnnotations;

namespace puc_projeto_eixo_2.Models
{
    public class PerfilViewModel
    {
        public Usuario Usuario { get; set; }
        public bool IsProprietario { get; set; }
        public List<Treino> TreinosCriados { get; set; } = new List<Treino>();
    }

    public class EditarPerfilViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome completo � obrigat�rio.")]
        [Display(Name = "Nome Completo")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O nome de usu�rio � obrigat�rio.")]
        [Display(Name = "Nome de Usu�rio")]
        public string NomeDeUsuario { get; set; }

        [Required(ErrorMessage = "O e-mail � obrigat�rio.")]
        [EmailAddress(ErrorMessage = "E-mail inv�lido.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "O g�nero � obrigat�rio.")]
        [Display(Name = "G�nero")]
        public Genero Genero { get; set; }

        [Required(ErrorMessage = "O estado � obrigat�rio.")]
        [Display(Name = "Estado")]
        public Estado Estado { get; set; }

        [Required(ErrorMessage = "A cidade � obrigat�ria.")]
        [Display(Name = "Cidade")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "A data de nascimento � obrigat�ria.")]
        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime Nascimento { get; set; }

        [Display(Name = "Nova Senha")]
        [DataType(DataType.Password)]
        public string? NovaSenha { get; set; }

        [Display(Name = "Confirmar Nova Senha")]
        [DataType(DataType.Password)]
        [Compare("NovaSenha", ErrorMessage = "As senhas n�o coincidem.")]
        public string? ConfirmarNovaSenha { get; set; }

        [Display(Name = "Foto de Perfil")]
        public string? FotoPerfil { get; set; }
    }
}