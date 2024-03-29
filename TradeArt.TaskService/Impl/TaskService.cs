﻿using TradeArt.Interfaces;
using System.Security.Cryptography;
using TradeArt.Core;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TradeArt.TaskService.Impl
{
    public class TaskService : ITaskService
    {
        public string InvertText(string text)
        {
            if (text.Length <= 1) return text;

            var arr = text.ToCharArray();
            Array.Reverse(arr);
            return string.Join("", arr);
        }

        public async Task<bool> FunctionA()
        {
            var list = new List<Task<bool>>();
            for (int i = 1; i < 1001; i++)
            {
                list.Add(Task.Run(() => FunctionB(i)));
            }
            await Task.WhenAll(list);
            return list.All(x => x.Status == TaskStatus.RanToCompletion);
        }

        private async Task<bool> FunctionB(int data)
        {
            await Task.Delay(100); //To behave as processing data that comes from A
            return true;
        }

        public Result<string> CalculateSHA256Hash(string filePath)
        {
            try
            {
                using var filestream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                using var sha256 = SHA256.Create();
                var hash = sha256.ComputeHash(filestream);
                var hashString = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                return Result<string>.AsSuccess(hashString);
            }
            catch(Exception ex)
            {
                return Result<string>.AsFailure(ex.Message);
            }
        }

    }
}
