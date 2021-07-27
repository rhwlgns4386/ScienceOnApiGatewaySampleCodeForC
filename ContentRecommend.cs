using System.Net;
using System;
using System.IO;
class ContentRecommend
{
    private static string target = "RECOMMEND";
    private static string clientID="Your ClientId";
    private static string accessToken="Your AccessToken";
    public static void Main_a()
    {

        string recomType = "merge";
        string cn = "NART95824392";
        string uid = "cchh7549";

        /** 추천을 할려는 유형,cn,uid를 입력 받아 콘텐츠 추천 api에 request를 요청하고 response를 받는다. */
        string response = getRecommendItem(recomType,cn,uid);

        Console.WriteLine(response);

    }

    /**
     * @brief 추천 api를 사용하기 위한 함수
     * @return String:요청을 받은 xml or json값
     * @param recomType:추천 유형(user,item,content,merge)
     * @param cn:선택한 콘텐츠 ID
     * @param uid:로그인 이용자 ID
     */
    private static string getRecommendItem(string recomType,string cn,string uid)
    {
        string target_URL ="https://apigateway.kisti.re.kr/openapicall.do?" +
                "client_id=" +clientID+
                "&token="+accessToken+"" +
                "&version=1.0" +
                "&action=browse" +
                "&target=" +target+
                "&recomType="+recomType+
                "&cn="+cn+
                "&uid="+uid;

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