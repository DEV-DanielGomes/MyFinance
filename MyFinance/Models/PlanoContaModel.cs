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
    public class PlanoContaModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe a descrição")]
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public int Usuario_Id { get; set; }

        public IHttpContextAccessor HttpContextAccessor { get; set; }

        //Refatoração (Diminuindo a declaração em vão de varias variáveis)
        //Coloco a função sem parametro dentro da variável que deve receber o valor
            private string IdUsuarioLogado()
        {
            return HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado"); //Classe de injenção de dependência
        }

        public PlanoContaModel()
        {

        }
        //Injeção de dependencia (Não necessita mais de usar o NEW - Diminui o aclopamento)
        //Primeiro ajustar na class Statup Singlaton// usar o ID da seção na variável Id_Usuario_Logado. 
        //Recebe o contexto da variavél de sessão
        public PlanoContaModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public List<PlanoContaModel> ListaPlanoConta()  //Criação de uma lista que possui estes atributos - método
        {
            List<PlanoContaModel> lista = new List<PlanoContaModel>(); //criando uma lista
            PlanoContaModel item; //preenchendo e colocando na lista Dados
                        
            string sql = $"SELECT ID, DESCRICAO, TIPO, USUARIO_ID FROM PLANO_CONTAS WHERE USUARIO_ID ='{IdUsuarioLogado()}'"; //Interpolação de classes
            DAL objdal = new DAL();
            DataTable dt = objdal.RetDataTable(sql); //classe de acesso a dados

            for (int i = 0; i < dt.Rows.Count; i++) //vai de zero até a quantidade linhas com dados
            {
                item = new PlanoContaModel(); //Aqui chamo minha lista Contamodel do meu item
                item.Id = int.Parse(dt.Rows[i]["ID"].ToString()); //Adicionando ao item o valor da linha no indice i e coluna ID.
                item.Descricao = dt.Rows[i]["Descricao"].ToString();
                item.Tipo = dt.Rows[i]["Tipo"].ToString();
                item.Usuario_Id = int.Parse(dt.Rows[i]["USUARIO_ID"].ToString());
                lista.Add(item); //Adicionando o elemento na lista (estanciei em cima) 
            }
            return lista;
        }

        public void Insert()
        { //Injeção de dependência
            string sql = "";
            if (Id == 0)
            {
                sql = $"INSERT INTO PLANO_CONTAS (DESCRICAO, TIPO, USUARIO_ID) VALUES('{Descricao}','{Tipo}','{IdUsuarioLogado()}')"; //Interpolação de string
            }
            else
            {
                sql = $"UPDATE PLANO_CONTAS SET DESCRICAO = '{Descricao}', TIPO='{Tipo}' WHERE USUARIO_ID='{IdUsuarioLogado()}' AND ID='{Id}'"; //Interpolação de string
            }
            DAL objdal = new DAL(); //Conexão com a classe DAO 
            objdal.ExcutaComandoSQL(sql); //Executando o SQL acima. 

        }
        //Excluir Conta
        public void Excluir(int id_conta) //Mesmo valor que está na view de mesmo nome
        {
            new DAL().ExcutaComandoSQL("DELETE FROM PLANO_CONTAS WHERE ID = " + id_conta);
        }

        //Editar Conta -> Carrego os dados na mesma view do cadastro

        public PlanoContaModel CarregarRegistro(int? id)
        {
            PlanoContaModel item = new PlanoContaModel();
                     
            string sql = $"SELECT ID, DESCRICAO, TIPO, USUARIO_ID FROM PLANO_CONTAS WHERE USUARIO_ID ='{IdUsuarioLogado()}' AND ID = '{id}'"; //Interpolação de classes
            DAL objdal = new DAL();
            DataTable dt = objdal.RetDataTable(sql); //classe de acesso a dados

            for (int i = 0; i < dt.Rows.Count; i++) //vai de zero até a quantidade linhas com dados
            {
                item = new PlanoContaModel(); //Aqui chamo minha lista Contamodel do meu item
                item.Id = int.Parse(dt.Rows[i]["ID"].ToString()); //Adicionando ao item o valor da linha no indice i e coluna ID.
                item.Descricao = dt.Rows[i]["Descricao"].ToString();
                item.Tipo = dt.Rows[i]["Tipo"].ToString();
                item.Usuario_Id = int.Parse(dt.Rows[i]["USUARIO_ID"].ToString());
            }
            return item;
        }
    }
}
