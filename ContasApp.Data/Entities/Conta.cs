using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContasApp.Data.Entities
{
    public class Conta
    {
        #region Atributos

        private Guid _id;
        private string? _nome;
        private DateTime _data;
        private decimal _valor;
        private int _tipo;
        private string? _observacoes;
        private Guid _categoriaId;
        private Guid _usuarioId;
        private Categoria? _categoria;

        #endregion

        #region Propriedades

        public Guid Id { get => _id; set => _id = value; }
        public string? Nome { get => _nome; set => _nome = value; }
        public DateTime Data { get => _data; set => _data = value; }
        public decimal Valor { get => _valor; set => _valor = value; }
        public int Tipo { get => _tipo; set => _tipo = value; }
        public string? Observacoes { get => _observacoes; set => _observacoes = value; }
        public Guid CategoriaId { get => _categoriaId; set => _categoriaId = value; }
        public Guid UsuarioId { get => _usuarioId; set => _usuarioId = value; }
        public Categoria? Categoria { get => _categoria; set => _categoria = value; }

        #endregion
    }
}
