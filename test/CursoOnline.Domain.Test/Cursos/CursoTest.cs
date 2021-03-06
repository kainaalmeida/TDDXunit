﻿using Bogus;
using CursoOnline.Domain.Cursos;
using CursoOnline.Domain.Test._Builders;
using CursoOnline.Domain.Test._Util;
using ExpectedObjects;
using System;
using Xunit;
using Xunit.Abstractions;

namespace CursoOnline.Domain.Test.Cursos
{
    public class CursoTest : IDisposable
    {
        private readonly ITestOutputHelper _outPut;
        private readonly string _nome;
        private readonly double _cargaHoraria;
        private readonly PublicoAlvo _publicoAlvo;
        private readonly double _valor;
        private readonly string _descricao;

        //Setup
        public CursoTest(ITestOutputHelper outPut)
        {
            _outPut = outPut;
            _outPut.WriteLine("teste sendo executado");

            var faker = new Faker();

            _nome = faker.Random.Word();
            _cargaHoraria = faker.Random.Double(500,1000);
            _publicoAlvo = PublicoAlvo.Estudante;
            _valor = faker.Random.Double(100,1000);
            _descricao = faker.Lorem.Paragraph();

        }

        [Fact(DisplayName = "DeveCriarCurso")]
        public void DeveCriarCurso()
        {
            var cursoEsperado = new
            {
                Nome = _nome,
                CargaHoraria = _cargaHoraria,
                PublicoAlvo = _publicoAlvo,
                Valor = _valor,
                Descricao = _descricao
            };

            var curso = new Curso(cursoEsperado.Nome, cursoEsperado.Descricao, cursoEsperado.CargaHoraria, cursoEsperado.PublicoAlvo, cursoEsperado.Valor);

            cursoEsperado.ToExpectedObject().ShouldMatch(curso);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NaoDeveCursoTerUmNomeInvalido(string nomeInvalido)
        {
            Assert.Throws<ArgumentException>(() =>
            CursoBuilder.Novo().ComNome(nomeInvalido).Build())
                .ComMensagem("Nome inválido");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        [InlineData(-100)]
        public void NaoDeveCursoTerUmaCargaHorariaMenorQue1(double cargaHorariaInvalida)
        {
            Assert.Throws<ArgumentException>(() =>
            CursoBuilder.Novo().ComCargaHoraria(cargaHorariaInvalida).Build())
                .ComMensagem("Carga horária inválida");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        [InlineData(-100)]
        public void NaoDeveCursoTerUmValorMenorQue1(double valorInvalido)
        {
            Assert.Throws<ArgumentException>(() =>
            CursoBuilder.Novo().ComValor(valorInvalido).Build())
                .ComMensagem("Valor inválido");
        }

        //Cleanup
        public void Dispose()
        {
            _outPut.WriteLine("Cleanup");
        }
    }
}
