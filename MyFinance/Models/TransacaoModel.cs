using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using global::MyFinance.Util;
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

    public class TransacaoModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Informe a Data!")]
        public string Data { get; set; }
        public string DataFinal { get; set; } //filtro
        public string Tipo { get; set; }
        public double Valor { get; set; }
        [Required(ErrorMessage = "Informe a descrição")]
        public string Descricao { get; set; }
        public int Conta_Id { get; set; }
        public string Nome_Conta { get; set; }
        public int Plano_Conta_Id { get; set; }
        public string Descricao_Plano_Conta { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public TransacaoModel()
        {

        }
        //Injeção de dependencia (Não necessita mais de usar o NEW - Diminui o aclopamento)
        //Primeiro ajustar na class Statup Singlaton// usar o ID da seção na variável Id_Usuario_Logado. 
        //Recebe o contexto da variavél de sessão
        public TransacaoModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public List<TransacaoModel> ListaTransacao()  //Criação de uma lista que possui estes atributos - método
        {
            List<TransacaoModel> lista = new List<TransacaoModel>(); //criando uma lista
            TransacaoModel item; //preenchendo e colocando na lista Dados

            //Utilizado pela View extrato:
            string filtro = "";
            if ((Data != null) && (DataFinal != null))
            {
                filtro += $" and t1.Data >='{DateTime.Parse(Data).ToString("yyyy/MM/dd")}' and t1.Data <='{DateTime.Parse(DataFinal).ToString("yyyy/MM/dd")}'";
            }
            if (Tipo != null)
            {
                if (Tipo != "A")
                {
                    filtro += $" and t1.Tipo = '{Tipo}'";

                }
            }
            if (Conta_Id != 0)
            {
                filtro += $" and t1.Conta_Id = '{Conta_Id}'";
            }
            //fim

            string Id_Usuario_Logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = $"select t1.Id, t1.Data, t1.Tipo, t1.Valor, t1.Descricao as Historico," +
                "t1.Conta_Id,t2.Nome as conta, t1.Plano_Contas_Id, t3.descricao as Plano_Contas " +
                "from transacao as t1 inner join conta as t2 on t1.Conta_Id = t2.Id inner join " +
                "plano_contas as t3 on t1.Plano_Contas_Id = t3.Id  " +
                $"where t1.usuario_id ='{Id_Usuario_Logado}' {filtro} order by t1.Data desc limit 10"; //Interpolação de classes
            DAL objdal = new DAL();
            DataTable dt = objdal.RetDataTable(sql); //classe de acesso a dados

            for (int i = 0; i < dt.Rows.Count; i++) //vai de zero até a quantidade linhas com dados
            {
                item = new TransacaoModel(); //Aqui chamo minha lista Contamodel do meu item
                item.Id = int.Parse(dt.Rows[i]["ID"].ToString()); //Adicionando ao item o valor da linha no indice i e coluna ID.
                item.Data = DateTime.Parse(dt.Rows[i]["Data"].ToString()).ToString("dd/MM/yyyy");
                item.Descricao = dt.Rows[i]["Historico"].ToString();
                item.Tipo = dt.Rows[i]["Tipo"].ToString();
                item.Valor = double.Parse(dt.Rows[i]["Valor"].ToString());
                item.Conta_Id = int.Parse(dt.Rows[i]["Conta_Id"].ToString());
                item.Nome_Conta = dt.Rows[i]["conta"].ToString();
                item.Plano_Conta_Id = int.Parse(dt.Rows[i]["Plano_Contas_Id"].ToString());
                item.Descricao_Plano_Conta = dt.Rows[i]["Plano_Contas"].ToString();
                lista.Add(item); //Adicionando o elemento na lista (estanciei em cima) 
            }
            return lista;
        }
        //Insert 
        public void Insert()
        {
            string Id_Usuario_Logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado"); //Injeção de dependência
            string sql = "";
            if (Id == 0)
            {
                sql = "INSERT INTO TRANSACAO (DATA, TIPO, DESCRICAO, VALOR, CONTA_ID, PLANO_CONTAS_ID, USUARIO_ID) " +
                    $"VALUES('{DateTime.Parse(Data).ToString("yyyy/MM/dd")}','{Tipo}','{Descricao}','{Valor}','{Conta_Id}','{Plano_Conta_Id}','{Id_Usuario_Logado}')"; //Interpolação de string
            }
            else
            {
                sql = $"UPDATE TRANSACAO SET DATA='{DateTime.Parse(Data).ToString("yyyy/MM/dd")}', DESCRICAO = '{Descricao}', " +
                    $"TIPO='{Tipo}', VALOR='{Valor}', CONTA_ID='{Conta_Id}', PLANO_CONTAS_ID ='{Plano_Conta_Id}' WHERE USUARIO_ID='{Id_Usuario_Logado}' AND ID='{Id}'"; //Interpolação de string
            }
            DAL objdal = new DAL(); //Conexão com a classe DAO 
            objdal.ExcutaComandoSQL(sql); //Executando o SQL acima. 

        }

        public TransacaoModel CarregarRegistro(int? id)  //Criação de uma lista que possui estes atributos - método
        {
            TransacaoModel item; //preenchendo e colocando na lista Dados

            string Id_Usuario_Logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = "select t1.Id, t1.Data, t1.Tipo, t1.Valor, t1.Descricao as Historico," +
                "t1.Conta_Id,t2.Nome as conta, t1.Plano_Contas_Id, t3.descricao as Plano_Contas " +
                "from transacao as t1 inner join conta as t2 on t1.Conta_Id = t2.Id inner join " +
                "plano_contas as t3 on t1.Plano_Contas_Id = t3.Id  " +
                $"where t1.usuario_id ='{Id_Usuario_Logado}' and t1.Id='{id}' "; //Interpolação de classes

            DAL objdal = new DAL();
            DataTable dt = objdal.RetDataTable(sql); //classe de acesso a dados

            item = new TransacaoModel(); //Aqui chamo minha lista transacaomodeu do meu item
            item.Id = int.Parse(dt.Rows[0]["ID"].ToString()); //Adicionando ao item o valor da linha no indice i e coluna ID.
            item.Data = DateTime.Parse(dt.Rows[0]["Data"].ToString()).ToString("dd/MM/yyyy");
            item.Descricao = dt.Rows[0]["Historico"].ToString();
            item.Tipo = dt.Rows[0]["Tipo"].ToString();
            item.Valor = double.Parse(dt.Rows[0]["Valor"].ToString());
            item.Conta_Id = int.Parse(dt.Rows[0]["Conta_Id"].ToString());
            item.Nome_Conta = dt.Rows[0]["conta"].ToString();
            item.Plano_Conta_Id = int.Parse(dt.Rows[0]["Plano_Contas_Id"].ToString());
            item.Descricao_Plano_Conta = dt.Rows[0]["Plano_Contas"].ToString();

            return item;
        }

        public void Excluir(int id) //Mesmo valor que está na view de mesmo nome
        {
            new DAL().ExcutaComandoSQL("DELETE FROM TRANSACAO WHERE ID = " + id);
        }

    }

    public class Dashboard //Classe auxiliar 
    {
        public double Total { get; set; }
        public string PlanoConta { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public Dashboard()
        {
        }
        public Dashboard(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public List<Dashboard> RetornarDadosGraficoPie() //Aqui é uma função para retornar uma lista da classe Dashboard
        {



            List<Dashboard> lista = new List<Dashboard>();//Aqui é a criação da lista Dashboard 
            Dashboard item; // Aqui digo que um item pertence a lista
            string Id_Usuario_Logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = "select sum(t1.Valor) as ValorTotal,t2.Descricao from transacao t1 join plano_contas t2 on t1.Plano_Contas_Id = t2.id " +
                $"where  t1.usuario_id ='{Id_Usuario_Logado}' and t1.Tipo = 'D' group by t2.Descricao; "; //Interpolação de string
            DAL objDAL = new DAL();
            DataTable dt = new DataTable();
            dt = objDAL.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++) // Vai pecorrer meu DataTable até o valor ser zero
            {
                item = new Dashboard();
                item.Total = double.Parse(dt.Rows[i]["ValorTotal"].ToString());
                item.PlanoConta = dt.Rows[i]["Descricao"].ToString();
                lista.Add(item);

            }
            return lista;
        }
    }
}
