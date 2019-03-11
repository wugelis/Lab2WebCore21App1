# Gelis - 程式設計訓練營 - 跨平台的 Web API Framework 框架開發

# 課程範例程式 POST

還記得小編在台中的 .NET Conf 2018 中所分享的『使用 .NET Standard 開發跨平台應用程式』的課程嗎？在課程當中，我除了將 .NET Standard 2.0 的來龍去脈完整的介紹一遍之外，現場我也 Live Demo 了一段使用我所使用 EasyArchitect 為基底而使用 .NET Standard 2.0 改寫的跨平台 Web API Framework 框架『StdEasyArchitect.Web.WebApiHostBase』現場的直接開一個完全【關注點分離】的 Web API 應用程式，這個架構包含：

(1). BO (Business Object)
(2). ApiHostBase （.NET Standard 2.0）
(3). ASP.NET Core 2.1 的網站

在這個跨平台的架構當中，BO 完全可以是一個乾淨的 .NET Core Lib 專案，完全的（獨立進行開發／獨立部署）。

## 課程大綱 (Agenda)：
● 談『商業邏輯導向』到『服務導向架構 SOA』架構
● 從 Web API 設計談 Clean Architecture 的軟體架構設計
● 談原本的 EasyArchitect Framework 設計目標
● 解決跨平台 Lib 共享問題 的（.NET Standard）
● 客製化的 Web API 伺服器服務框架
● 實作：使用 .NET Standard 從無到有打造跨平台的 Web Api 框架

詳細課程內容可參考：
https://mystudyway.kktix.cc/events/softshare-web-api-framework

此專案為該課程所有原始範例程式

## 本軟體授權方式
本軟體使用 MIT 授權條款（Massachusetts Institute of Technology）, Copyright (C) 2017 Gelis Wu.

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
