using System.Net;
using System;
using System.IO;

namespace Detailed
{
    class AuthorDetailed
    {

        private static string target = "AUTHOR";
        private static string clientID = "Your ClientId";
        private static string accessToken = "Your AccessToken";
        public static void Main_a()
        {
            string cn = "ADPER0000032785";

            /** cn을 입력하여 전자전거 상세보기 api에 request를 요청하고 response를 받는다. */
            string response = select(cn);

            Console.WriteLine(response);

        }

        /**
         * @brief 각각의 상세보기 api를 사용하기 위한 함수
         * @return String:요청을 받은 xml값
         * @param cn:해당 CN 번호 입력
         */
        private static string select(string cn)
        {
            string target_URL = "https://apigateway.kisti.re.kr/openapicall.do?" +
                    "client_id=" + clientID +
                    "&token=" + accessToken +
                    "&version=1.0" +
                    "&action=browse" +
                    "&target=" + target +
                    "&cn=" + cn;
            string response = getResponse(target_URL);

            return response;

        }


        /**
         * @brief 서버로 request요청을 보내고 그에 맞는 response를 받는
         * @return String:요청을 받은 xml or json값
         * @param target_URL:요청을 보낼 url
         */
        public static string getResponse(string target_URL)
        {
            string responseText = string.Empty;
            Console.WriteLine(target_URL);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(target_URL);
            request.Method = "GET";
            request.Timeout = 30 * 1000;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    HttpStatusCode statusCode = response.StatusCode;
                    Console.WriteLine(statusCode);

                    Stream respStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(respStream))
                    {
                        responseText = sr.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return responseText;
        }
    }
}