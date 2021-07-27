using System.Net;
using System;
using System.IO;
using System.Web;
class PaperTOC
{
    private static string target = "VOLUME";
    private static string clientID = "Your ClientId";
    private static string accessToken = "Your AccessToken";
    public static void Main_a()
    {

        string cn = "NJOU00023797";
        string volno = "3";

        /** cn과 volno를 입력받아 kisti의 권호정보를 반환 받습니다. */
        string response = getTOC(cn, volno);

        Console.WriteLine(response);

    }

    /**
     * @brief 권호 TOC API를 사용하기 위한 함수
     * @return String:요청을 받은 xml값
     * @param cn:선택한 저널CN를 입력
     * @param volno:선택한 권호번호를 입력
     */
    private static string getTOC(string cn, string volno)
    {
        string target_URL = "https://apigateway.kisti.re.kr/openapicall.do?" +
                "client_id=" + clientID +
                "&token=" + accessToken +
                "&version=1.0" +
                "&action=toc" +
                "&target=" + target +
                "&cn=" + cn +
                "&volno=" + volno;

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
            Console.WriteLine(e.Message.ToString());
        }
        return responseText;
    }
}