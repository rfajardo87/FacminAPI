using System;
using Models;

namespace App
{
    public class Application
    {
        internal Ctx _ctx;
        internal FCtx _fctx;

        public Application()
        {
            this._ctx = new Ctx();
            this._fctx = new FCtx();
        }
    }
}
