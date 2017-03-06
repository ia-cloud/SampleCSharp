// Copyright (c) 2016 K.T.System Co., Ltd. All Rights Reserved. See License in the project root.

using System;
using System.Collections.Generic;
using IACloudCs.Https;
using IACloudCs.ObjectModels;

namespace IaCloudCsSample
{
    class Program
    {
        // テストデータをクラウド上のCCSへPOSTするサンプルです
        static void Main(string[] args)
        {
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
                var productionResult = new IACloudObject
                {
                    ObjectKey = "com.ia-cloud.sample.fds.win-cs",
                    TimeStamp = DateTime.Now,
                    ObjectDescription = "Production Result of M001",
                    ObjectContent = new ObjectContent
                    {
                        ContentType = "ProductionResult",
                        ContentData = new List<ContentDatum>
                        {
                            new ContentDatum { DataName = "PlannedQuantity", DataValue = 1000 },
                            new ContentDatum { DataName = "ProducedQuantity", DataValue = 990 },
                            new ContentDatum { DataName = "GoodQuantity", DataValue = 988 },
                            new ContentDatum { DataName = "ScrapQuantity", DataValue = 2 },
                            new ContentDatum { DataName = "StartDate", DataValue = "2016-10-01T09:45+09:00" }
                        }
                    }
                };

                // サーバへPOST
                client.Store(productionResult);
            }
        }
    }
}
