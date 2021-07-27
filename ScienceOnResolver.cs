using System.Net;
using System;
using System.IO;
using System.Web;
class ScienceOnResolver
{
    private static string target="RESOLVER";
    private static string clientID="Your ClientId";
    private static string accessToken="Your AccessToken";
    public static void Main_a()
    {

        string paperTitle="Association of p53 Expression with Metabolic Features of Stage I Non-Small Cell Lung Cancer";
        string query = HttpUtility.UrlEncode(paperTitle);

        /** cn과 volno를 입력받아 kisti의 권호정보를 반환 받습니다. */
        string response= getSearchResults(query);

        Console.WriteLine(response);
        
    }

    /**
     * @brief ScienceON 링크리졸버 API사용 함수
     * @return String:요청을 받은 xml값
     * @param paperTitle:정보를 원하는 논문 명
     */
    private static string getSearchResults(string paperTitle){
        string target_URL="https://apigateway.kisti.re.kr/openapicall.do?" +
                "client_id=" +clientID+
                "&token=" +accessToken+
                "&version=1.0" +
                "&action=resolver" +
                "&target=" +target+
                "&atitle="+paperTitle;

        string response=getResponse(target_URL);

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