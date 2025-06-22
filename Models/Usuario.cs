using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace puc_projeto_eixo_2.Models
{
    [Table("Table_Usuario")]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        public string NomeDeUsuario { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        public string Email { get; set; }

        public string? Telefone { get; set; }

        [Required(ErrorMessage = "O gênero é obrigatório.")]
        public Genero Genero { get; set; }

        [Required(ErrorMessage = "O estado é obrigatório.")]
        public Estado Estado { get; set; }

        [Required(ErrorMessage = "A cidade é obrigatória.")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime Nascimento { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Required(ErrorMessage = "A confirmação de senha é obrigatória.")]
        [DataType(DataType.Password)]
        [Compare("Senha", ErrorMessage = "As senhas não coincidem.")]
        [NotMapped]
        public string ConfirmarSenha { get; set; }

        public Perfil Perfil { get; set; }

        // Novo campo para foto de perfil
        public string? FotoPerfil { get; set; }

        // Propriedade de navegação para treinos criados pelo usuário
        public virtual ICollection<Treino> Treinos { get; set; } = new List<Treino>();
    }

    public enum Genero
    {
        Masculino,
        Feminino,
        Outro
    }
    public enum Estado
    {
        Acre,
        Alagoas,
        Amapá,
        Amazonas,
        Bahia,
        Ceará,
        DistritoFederal,
        EspíritoSanto,
        Goiás,
        Maranhão,
        MatoGrosso,
        MatoGrossodoSul,
        MinasGerais,
        Pará,
        Paraíba,
        Paraná,
        Pernambuco,
        Piauí,
        RiodeJaneiro,
        RioGrandedoNorte,
        RioGrandedoSul,
        Rondônia,
        Roraima,
        SantaCatarina,
        SãoPaulo,
        Sergipe,
        Tocantins
    }
    public enum Perfil
    {
        Admin,
        User
    }
}