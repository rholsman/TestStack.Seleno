﻿using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.Specifications.Assertions
{
    public class ElementAssert
    {
        private readonly UiComponent _component;
        private readonly By _selector;
        private readonly ICamera _camera;

        public ElementAssert(UiComponent component, By selector, ICamera camera)
        {
            _component = component;
            _selector = selector;
            _camera = camera;
        }

        RemoteWebDriver Browser
        {
            get
            {
                return _component.Browser;
            }
        }

        public ElementAssert DoNotExist(string message = null)
        {
            try
            {
                Browser.FindElement(_selector);
            }
            catch (NoSuchElementException)
            {
                return this;
            }

            if (string.IsNullOrEmpty(message))
                message = string.Format("'{0}' was in fact found!", _selector);

            throw new SelenoException(message);
        }

        public ElementAssert Exist(string message = null)
        {
            try
            {
                Browser.FindElement(_selector);
            }
            catch (NoSuchElementException ex)
            {
                throw new SelenoException(message, ex);
            }

            return this;
        }

        public ElementAssert ConformTo(Action<IEnumerable<IWebElement>> assertion)
        {
            try
            {
                var elements = Browser.FindElements(_selector);
                assertion(elements);
            }
            catch (Exception)
            {
                _camera.TakeScreenshot();
                throw;
            }

            return this;
        }
    }
}