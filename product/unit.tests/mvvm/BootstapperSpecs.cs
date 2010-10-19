using System;
using Machine.Specifications;
using MVPtoMVVM.mvvm;
using StructureMap;

namespace unit.tests.mvvm
{
    public class BootstapperSpecs
    {
        [Subject(typeof (Bootstrap))]
        public class when_initializing_the_bootstrapper
        {
            Establish context = () => { new Bootstrap().Execute(); };

            It should = () => { Console.Out.WriteLine(ObjectFactory.WhatDoIHave()); };
        }
    }
}