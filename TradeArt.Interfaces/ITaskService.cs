﻿namespace TradeArt.Interfaces
{
    public interface ITaskService
    {
        string InvertText(string text);
        Task<bool> FunctionA();
    }
}