using System.Net;
using System;
using System.IO;
using System.Web;

namespace Search
{
    class SearchFunction
    {
        private static string target = "FUNCTION";
        private static string clientID = "Your ClientId";
        private static string accessToken = "Your AccessToken";
        public static void Main_a()
        {
            /**
             * 검색어를 인코딩 합니다.
             * 검색어 필드에 관한것은 ScienceOn Api GateWay를 참고 해주세요.
             */
            string query = HttpUtility.UrlEncode("{\"BI\":\"논문\"}");

            /** 검색할 쿼리를 입력하여 기능 검색 api에 request를 요청하고 response를 받는다. */
            string response = getSearchResults(query);

            Console.WriteLine(response);

        }

        /**
         * @brief 각각의 검색 api를 사용하기 위한 함수
         * @return String:요청을 받은 xml값
         * @param query:url인코딩된 검색 데이터 입력
         */
        private static string getSearchResults(string query)
        {
            string target_URL = "https://apigateway.kisti.re.kr/openapicall.do?" +
                    "client_id=" + clientID +
                    "&token=" + accessToken +
                    "&version=1.0" +
                    "&action=search" +
                    "&target=" + target +
                    "&searchQuery=" + query;

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