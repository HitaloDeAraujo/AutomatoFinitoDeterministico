using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AutomatoFinito
{
    /// <summary>
    /// Automato Finito Deterministico configurado para validacao de marcacoes como das linguagens HTML, XML e AIML
    /// </summary>
    class AutomatoFinito
    {
        private static int _indice, _controloDeTags = 0;
        private static List<string> _seqDeTags = new List<string>();
        public static bool entradaValida { get; set; }

        //Matriz para tabela de transicoes
        private static string[,] _tabelaDeTransicoes = new string[32, 5];

        /// <summary>
        /// Identifica se o caractere se encaixa no padrao
        /// </summary>
        private static bool Simbolo(char caractere, string padrao)
        {
            if (caractere != '<' && caractere != '>' && padrao == "c")
                return true;
            else if (caractere == '<' && padrao == "<")
                return true;
            else if (caractere == '>' && padrao == ">")
                return true;
            else if (caractere == '/' && padrao == "/")
                return true;
            else if (padrao == "*")
                return true;
            else
                return false;
        }

        /// <summary>
        /// Identifica a acao semantica
        /// </summary>
        private static void AcaoSemantica(string Acao)
        {
            switch (Acao)
            {
                case "a1": _controloDeTags++; break;
                case "a2": if (_controloDeTags > 0) _controloDeTags--; break;

                default: break;
            }

            _indice++;    //Novo caractere
        }

        /// <summary>
        /// Carregar tabela de transicoes
        /// A tabela esta construida para identificar marcacoes como as das linguagens HTML, XML e AIML
        /// </summary>
        private static void CarregarTabelaDeTransicoes()
        {
            //Estado Atual                   //Simbolo Lido                  //Proximo Estado                 //Proxima Transicao             //Acoes Semanticas
            _tabelaDeTransicoes[0, 0] = "s0"; _tabelaDeTransicoes[0, 1] = "<"; _tabelaDeTransicoes[0, 2] = "s1"; _tabelaDeTransicoes[0, 3] = "2"; _tabelaDeTransicoes[0, 4] = "";
            _tabelaDeTransicoes[1, 0] = "s0"; _tabelaDeTransicoes[1, 1] = "*"; _tabelaDeTransicoes[1, 2] = "s0"; _tabelaDeTransicoes[1, 3] = "0"; _tabelaDeTransicoes[1, 4] = "";


            _tabelaDeTransicoes[2, 0] = "s1"; _tabelaDeTransicoes[2, 1] = ">"; _tabelaDeTransicoes[2, 2] = "s2"; _tabelaDeTransicoes[2, 3] = "5"; _tabelaDeTransicoes[2, 4] = "a1";
            _tabelaDeTransicoes[3, 0] = "s1"; _tabelaDeTransicoes[3, 1] = "/"; _tabelaDeTransicoes[3, 2] = "s3"; _tabelaDeTransicoes[3, 3] = "7"; _tabelaDeTransicoes[3, 4] = "";
            _tabelaDeTransicoes[4, 0] = "s1"; _tabelaDeTransicoes[4, 1] = "*"; _tabelaDeTransicoes[4, 2] = "s1"; _tabelaDeTransicoes[4, 3] = "2"; _tabelaDeTransicoes[4, 4] = "";


            _tabelaDeTransicoes[5, 0] = "s2"; _tabelaDeTransicoes[5, 1] = "<"; _tabelaDeTransicoes[5, 2] = "s1"; _tabelaDeTransicoes[5, 3] = "2"; _tabelaDeTransicoes[5, 4] = "";
            _tabelaDeTransicoes[6, 0] = "s2"; _tabelaDeTransicoes[6, 1] = "*"; _tabelaDeTransicoes[6, 2] = "s2"; _tabelaDeTransicoes[6, 3] = "5"; _tabelaDeTransicoes[6, 4] = "";


            _tabelaDeTransicoes[7, 0] = "s3"; _tabelaDeTransicoes[7, 1] = ">"; _tabelaDeTransicoes[7, 2] = "s0"; _tabelaDeTransicoes[7, 3] = "0"; _tabelaDeTransicoes[7, 4] = "";
            _tabelaDeTransicoes[8, 0] = "s3"; _tabelaDeTransicoes[8, 1] = "*"; _tabelaDeTransicoes[8, 2] = "s4"; _tabelaDeTransicoes[8, 3] = "9"; _tabelaDeTransicoes[8, 4] = "a2";

            _tabelaDeTransicoes[9, 0] = "s4"; _tabelaDeTransicoes[9, 1] = ">"; _tabelaDeTransicoes[9, 2] = "s0"; _tabelaDeTransicoes[9, 3] = "0"; _tabelaDeTransicoes[9, 4] = "";
            _tabelaDeTransicoes[10, 0] = "s4"; _tabelaDeTransicoes[10, 1] = "*"; _tabelaDeTransicoes[10, 2] = "s4"; _tabelaDeTransicoes[10, 3] = "9"; _tabelaDeTransicoes[10, 4] = "";


            ////tagas uma por uma
            ////Estado Atual                   //Simbolo Lido                  //Proximo Estado                 //Proxima Transicao             //Acoes Semanticas
            //TabelaDeTransicoes[0, 0] = "s0"; TabelaDeTransicoes[0, 1] = "<"; TabelaDeTransicoes[0, 2] = "s1"; TabelaDeTransicoes[0, 3] = "2"; TabelaDeTransicoes[0, 4] = "";
            //TabelaDeTransicoes[1, 0] = "s0"; TabelaDeTransicoes[1, 1] = "*"; TabelaDeTransicoes[1, 2] = "s0"; TabelaDeTransicoes[1, 3] = "0"; TabelaDeTransicoes[1, 4] = "";


            //TabelaDeTransicoes[2, 0] = "s1"; TabelaDeTransicoes[2, 1] = ">"; TabelaDeTransicoes[2, 2] = "s2"; TabelaDeTransicoes[2, 3] = "5"; TabelaDeTransicoes[2, 4] = "a1";
            //TabelaDeTransicoes[3, 0] = "s1"; TabelaDeTransicoes[3, 1] = "/"; TabelaDeTransicoes[3, 2] = "s3"; TabelaDeTransicoes[3, 3] = "7"; TabelaDeTransicoes[3, 4] = "";
            //TabelaDeTransicoes[4, 0] = "s1"; TabelaDeTransicoes[4, 1] = "*"; TabelaDeTransicoes[4, 2] = "s1"; TabelaDeTransicoes[4, 3] = "2"; TabelaDeTransicoes[4, 4] = "";


            //TabelaDeTransicoes[5, 0] = "s2"; TabelaDeTransicoes[5, 1] = "<"; TabelaDeTransicoes[5, 2] = "s1"; TabelaDeTransicoes[5, 3] = "2"; TabelaDeTransicoes[5, 4] = "";
            //TabelaDeTransicoes[6, 0] = "s2"; TabelaDeTransicoes[6, 1] = "*"; TabelaDeTransicoes[6, 2] = "s2"; TabelaDeTransicoes[6, 3] = "5"; TabelaDeTransicoes[6, 4] = "";


            //TabelaDeTransicoes[7, 0] = "s3"; TabelaDeTransicoes[7, 1] = ">"; TabelaDeTransicoes[7, 2] = "s0"; TabelaDeTransicoes[7, 3] = "0"; TabelaDeTransicoes[7, 4] = "a2";
            //TabelaDeTransicoes[8, 0] = "s3"; TabelaDeTransicoes[8, 1] = "*"; TabelaDeTransicoes[8, 2] = "s3"; TabelaDeTransicoes[8, 3] = "7"; TabelaDeTransicoes[8, 4] = "";
        }

        /// <summary>
        /// Automato finito
        /// </summary>
        private static bool ProcessarSequencia(string Entrada)
        {
            //Carrega a tabela uma vez
            if (_tabelaDeTransicoes[0, 0] == null)
                CarregarTabelaDeTransicoes();

            //Define fim da entrada
            if (Entrada == "")
                return true;

            _indice = 0;

            //Lista de estados finais
            List<string> EstadosFinais = new List<string>();
            EstadosFinais.Add("s0");

            int Linha = 0;
            char EntradaAtual;
            string EstadoAtual = "s0";

            //Para cada caractere
            for (int i = 0; i < Entrada.Length; i++)
            {
                EntradaAtual = Entrada[i];

                if (Simbolo(EntradaAtual, _tabelaDeTransicoes[Linha, 1]))    //Verifica se a EntradaAtual corresponde ao padrao
                {
                    AcaoSemantica(_tabelaDeTransicoes[Linha, 4]);    //Usa acao semantica especifica

                    EstadoAtual = _tabelaDeTransicoes[Linha, 2];     //Novo estado atual

                    if (_tabelaDeTransicoes[Linha, 3] != "nenhum")           //Verifica se existe estado posterior
                        Linha = int.Parse(_tabelaDeTransicoes[Linha, 3]);    //Linha passa a ser a linha do proximo estado
                }
                else
                {
                    bool teste = false; //Variavel auxiliar

                    do  //Enquanto a linha estiver no estado atual
                    {
                        Linha = Linha + 1;  //Passa para a proxima linha

                        //Verifica se o proximo estado e igual ao estado atual e se a EntradaAtual corresponde ao padrao
                        if (EstadoAtual == _tabelaDeTransicoes[Linha, 0] && Simbolo(EntradaAtual, _tabelaDeTransicoes[Linha, 1]))
                        {
                            AcaoSemantica(_tabelaDeTransicoes[Linha, 4]);    //Usa acao semantica especifica

                            EstadoAtual = _tabelaDeTransicoes[Linha, 2];     //Novo estado atual

                            if (_tabelaDeTransicoes[Linha, 3] != "nenhum")           //Verifica se existe estado posterior
                                Linha = int.Parse(_tabelaDeTransicoes[Linha, 3]);    //Linha passa a ser a linha do proximo estado

                            teste = false;
                        }
                        else if ((Linha + 1 < 32) && EstadoAtual == _tabelaDeTransicoes[Linha + 1, 0])   //Verifica se a proxima linha existe e se o seu estado e o atual
                            teste = true;
                        else
                            return false;

                    } while (teste);    //Enquanto a linha estiver no estado atual
                }

                if (EstadosFinais.Contains(EstadoAtual) && _controloDeTags == 0)        //Verifica se o estado atual e algum dos estados finais
                {
                    string auxiliar = Entrada.Substring(0, i + 1);

                    _seqDeTags.Add(auxiliar);

                    return true;
                }
            }

            if (EstadosFinais.Contains(EstadoAtual) && _controloDeTags == 0)            //Verifica se o estado atual e algum dos estados finais
            {
                _seqDeTags.Add(Entrada);

                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Lista de sequencia valida
        /// </summary>
        public static List<string> SequenciaValidada()
        {
            return _seqDeTags;
        }

        /// <summary>
        /// Inicia tratamento lexico
        /// </summary>
        public static void Iniciar_AutomatoFinito(string entrada)
        {
            while (ProcessarSequencia(entrada))
            {
                if (entrada == "")
                {
                    entradaValida = true;
                    return;
                }

                if (_indice + 1 <= entrada.Length)
                    entrada = entrada.Substring(_indice);
                else
                    entrada = "";
            }

            entradaValida = false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            AutomatoFinito.Iniciar_AutomatoFinito("<a>quem esta ai?</a>");

            if(AutomatoFinito.entradaValida)
                Console.WriteLine("Entrada validada");
            else
                Console.WriteLine("A sequencia incorreta");

            foreach (var elementoValido in AutomatoFinito.SequenciaValidada())
                Console.WriteLine(elementoValido);

            Console.ReadKey();
        }
    }
}