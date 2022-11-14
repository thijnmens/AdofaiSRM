using System;

namespace AdofaiSRM.src
{
    internal class Translations
    {
        private static readonly string[] languages = new string[3] { "EN", "JP", "KR" };

        private static readonly string[] startupMessage = new string[3] {
            "Request songs with !srm <code>",
            "!srm <コード> でレベルをリクエストできます",
            "!srm <코드> 로 레벨을 요청하실 수 있습니다"
        };

        private static readonly string[] songNotFound = new string[3] {
            "Sorry, we could not find a song with id {0}. Did you make a typo?",
            "すみませんが、ID{0}のレベルが見つかりませんでした。誤字がないか確認してください。",
            "죄송하지만, ID {0}의 레벨을 찾을 수 없었습니다. 오타가 없는지 확인해 주세요."
        };

        private static readonly string[] addedToQueue = new string[3] {
            "I've sucessfully added {0} to the queue",
            "{0}を待機列に追加しました",
            "{0} 레벨을 대기열에 추가했습니다"
        };

        private static readonly string[] workshopOnlyMode = new string[3] {
            "WorkshopOnly mode is enabled, you can only request songs that are on the steam workshop",
            "WorkshopOnlyモードが有効です。Steamワークショップにあるレベルのみリクエストできます",
            "WorkshopOnly 모드가 켜져 있으며, Steam 창작마당에 있는 레벨만 신청하실 수 있습니다"
        };

        private static readonly string[] downloadingFile = new string[3] {
            "Downloading file... This might take a while",
            "ファイルをダウンロードしています... 少しお待ち下さい",
            "파일을 다운로드하고 있습니다... 잠시 기다려 주세요"
        };

        private static readonly string[] downloadError = new string[3] {
            "An error ocurred while downloading, is the download a direct link?",
            "ダウンロード中にエラーが発生しました。ファイルへの直接リンクか確認してください",
            "다운로드 중에 에러가 발생했습니다. 다운로드 링크가 다이렉트 링크인지 확인해 주세요."
        };

        private static readonly string[] uploadError = new string[3] {
           "An error ocurred while uploading, are the servers down?",
            "アップロード中にエラーが発生しました。サーバーが落ちているか配信者に確認してください。",
            "업로드 중에 에러가 발생했습니다. 서버가 꺼져 있는지 스트리머에게 확인해 주세요."
        };

        private static readonly string[] help = new string[3] {
            "!help | !srm <code> | !queue <?page>",
            "!help | !srm <コード> | !queue <?ページ>",
            "!help | !srm <코드> | !queue <?페이지>"
        };

        private static readonly string[] queueItems = new string[3] {
            "Items in queue: {0}",
            "待機中のレベル：{0}個",
            "대기 중인 레벨: {0}개"
        };

        private static readonly string[] queuePageOutOfRange = new string[3] {
            "This page does not exist",
            "このページは存在しません",
            "이 페이지는 존재하지 않습니다"
        };

        public static string GetTranslation(string id, string msg = "")
        {
            int i = Array.IndexOf(languages, Config.language);
            switch (id)
            {
                case "startupMessage":
                    return string.Format(startupMessage[i], msg);

                case "songNotFound":
                    return string.Format(songNotFound[i], msg);

                case "addedToQueue":
                    return string.Format(addedToQueue[i], msg);

                case "workshopOnlyMode":
                    return string.Format(workshopOnlyMode[i], msg);

                case "downloadingFile":
                    return string.Format(downloadingFile[i], msg);

                case "downloadError":
                    return string.Format(downloadError[i], msg);

                case "uploadError":
                    return string.Format(uploadError[i], msg);

                case "help":
                    return string.Format(help[i], msg);

                case "queueItems":
                    return string.Format(queueItems[i], msg);

                case "queuePageOutOfRange":
                    return string.Format(queuePageOutOfRange[i], msg);

                default: return $"Unkown translation \"{id}\"";
            }
        }
    }
}