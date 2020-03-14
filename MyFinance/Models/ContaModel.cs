using Microsoft.AspNetCore.Http;
using MyFinance.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinance.Models
{
    public class ContaModel
    {
        public int Id { get; set; }

        [Required (ErrorMessage ="Informe o nome da conta")]
        public string Nome { get; set; }
        [Required (ErrorMessage ="Informe o saldo da conta")]
        public double Saldo { get; set; }
        public int Usuario_Id { get; set; }

       public IHttpContextAccessor HttpContextAccessor { get; set; }
        public ContaModel()
        {

        }
        //Refatoração -> Passando a injeção de dependência
        private string IdUsuarioLogado()
        {
            return HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
        }
        //Injeção de dependencia (Não necessita mais de usar o NEW - Diminui o aclopamento) // Video 26
        //Primeiro ajustar na class Statup Singlaton// usar o ID da seção na variável Id_Usuario_Logado. 
        //Recebe o contexto da variavél de sessão
        public ContaModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        //Exibir uma listagem de contas 
        public List<ContaModel> ListaConta()  //Criação de uma lista que possui estes atributos - método
    {
        List<ContaModel> lista = new List<ContaModel>(); //criando uma lista
        ContaModel item; //preenchendo e colocando na lista
                        
        string sql = $"SELECT ID, NOME, SALDO, USUARIO_ID FROM CONTA WHERE USUARIO_ID ='{IdUsuarioLogado()}'"; //Interpolação de classes
        DAL objdal = new DAL();
        DataTable dt = objdal.RetDataTable(sql); //classe de acesso a dados

        for (int i = 0; i < dt.Rows.Count; i++) //vai de zero até a quantidade linhas com dados
        {
            item = new ContaModel(); //Aqui chamo minha lista Contamodel do meu item
            item.Id = int.Parse(dt.Rows[i]["ID"].ToString()); //Adicionando ao item o valor da linha no indice i e coluna ID.
            item.Nome = dt.Rows[i]["Nome"].ToString();
            item.Saldo = double.Parse(dt.Rows[i]["SALDO"].ToString());
            item.Usuario_Id = int.Parse(dt.Rows[i]["USUARIO_ID"].ToString());
            lista.Add(item); //Adicionando o elemento na lista (estanciei em cima) 
        }
        return lista;
        }
        //Inserir nova conta
        public void Insert()
        {
           
            string sql = $"INSERT INTO CONTA (NOME, SALDO, USUARIO_ID) VALUES('{Nome}','{Saldo}','{IdUsuarioLogado()}')"; //Interpolação de string
            DAL objdal = new DAL(); //Conexão com a classe DAO 
            objdal.ExcutaComandoSQL(sql); //Executando o SQL acima. 

        }        
        //Excluir Conta
        public void Excluir(int id_conta) //Mesmo valor que está na view (mesmo nome)
        {
            new DAL().ExcutaComandoSQL("DELETE FROM CONTA WHERE ID = " + id_conta);
        }
    }
}


