using MyFinance.Util;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MyFinance.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }

        [Required (ErrorMessage = "Nome Inválido")]
        public string Nome { get; set; }


        [Required(ErrorMessage = "E-mail Inválido")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "O e-mail informado é invalido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha Inválida")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Data de Nascimento Inválido")]
        public string Data_Nascimento { get; set; }

        public bool ValidaLogin()
        {
            string sql = $"SELECT ID, SENHA, NOME,EMAIL, DATA_NASCIMENTO FROM USUARIO WHERE EMAIL='{Email}' AND SENHA='{Senha}'";
            DAL objDAL = new DAL();
            DataTable dt = objDAL.RetDataTable(sql);

            if(dt != null)
            {
                if (dt.Rows.Count == 1)
                {
                    Id = int.Parse(dt.Rows[0]["ID"].ToString());
                    Nome = dt.Rows[0]["NOME"].ToString();
                    Data_Nascimento = dt.Rows[0]["DATA_NASCIMENTO"].ToString();
                    return true;
                }
            }

            return false;
        }

        public void RegistraUsuario()
        {
            string dataNascimento = DateTime.Parse(Data_Nascimento).ToString("yyyy/MM/dd");
            string sql = $"INSERT INTO USUARIO (NOME, EMAIL, SENHA, DATA_NASCIMENTO) VALUES ('{Nome}','{Email}', '{Senha}', '{Data_Nascimento}')";
            DAL objdal = new DAL();
            objdal.ExcutaComandoSQL(sql);
        }

    }
}
