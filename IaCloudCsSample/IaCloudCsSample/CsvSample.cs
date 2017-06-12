// Copyright (c) 2017 K.T.System Co., Ltd. All Rights Reserved. See License in the project root.

using System;
using System.Collections.Generic;
using System.Linq;
using IACloudCs.Https;
using IACloudCs.ObjectModels;
using Microsoft.VisualBasic.FileIO;

namespace IaCloudCsSample
{
    class CsvSample
    {
        // CSV形式のテストデータをクラウド上のCCSへPOSTするサンプルです
        public static void PostFromCsv()
        {
            // データを読み込んでia-cloudオブジェクトを作成する
            var csvData = ReadFromCsv(@"path\to\your\file").Select(line => new IACloudObject
            {
                ObjectContent = new ObjectContent
                {
                    ContentType = "ProductionResult",
                    ContentData = new List<ContentDatum>
                    {
                        new ContentDatum { DataName = "PlannedQuantity", DataValue = line.PlannedQty },
                        new ContentDatum { DataName = "ProducedQuantity", DataValue = line.ProducedQty },
                        new ContentDatum { DataName = "GoodQuantity", DataValue = line.GoodQty },
                        new ContentDatum { DataName = "ScrapQuantity", DataValue = line.ScrapQty },
                        new ContentDatum { DataName = "StartDate", DataValue = line.StartDate }
                    }
                }
            }).ToList();


            // クライアントクラスの生成
            using (var client = new HttpsClient(new JsonContentResolver()))
            {
                // ia-cloud関連の設定
                client.FDSKey = "ia-cloud";
                client.UserID = "ia-cloud";
                client.BasicAuthUserID = "ia-cloud-user1";
                client.BasicAuthPassword = "ia-cloud-passoword";
                client.ServiceUrl = "https://test.iacloud.com/iaCloudTest/v2.0";

                // 生産実績オブジェクトを作成
                var productionResult = new IACloudObjectArray
                {
                    ObjectKey = "com.ia-cloud.sample.fds.win-cs",
                    TimeStamp = DateTime.Now,
                    ObjectDescription = "Production Result of M001",
                    Length = csvData.Count,
                    // 上で作成したデータオブジェクトをセット
                    ObjectArray = csvData
                };

                // サーバへPOST
                client.Store(productionResult);
            }
        }

        // CSVファイルを読み込む
        private static IEnumerable<dynamic> ReadFromCsv(string filePath)
        {
            bool skipHeader = true;
            using(var parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    var col = parser.ReadFields();
                    if(skipHeader){
                        skipHeader = false;
                        continue;
                    }

                    yield return new
                    {
                        OrderNo = col[0],
                        CurrentTime = col[1],
                        PlannedQty = col[2],
                        ProducedQty = col[3],
                        GoodQty = col[4],
                        ScrapQty = col[5],
                        StartDate = col[6],
                    };
                }
            }
        }
    }
}
