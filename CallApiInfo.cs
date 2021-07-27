using Detailed;
using Search;
using System.Xml;
using System;
using System.Web;

class CallApiInfo
{
    public static void Main_a()
    {

        string cn = "322433";

        /** CallApiInfo의 실행을 위해 연구원상세보기 api를 호출합니다. */
        string response = ResearcherDetailed.select(cn);
        Console.WriteLine(response);

        XmlDocument xml=new XmlDocument();
        xml.LoadXml(response);
        
        XmlNodeList nodeList=xml["MetaData"]["recordList"].GetElementsByTagName("record");
        foreach(XmlNode record in nodeList){
            foreach(XmlNode item in record.ChildNodes){
                if(item.Attributes["metaCode"].InnerText.Equals("CallAPIInfo")){
                    if(item.ChildNodes[0].InnerText.Equals("API-001-01")){
                        string query=HttpUtility.UrlEncode(item.ChildNodes[2].InnerText);
                        string paperResponse=SearchPaper.getSearchResults(query);
                        Console.WriteLine(paperResponse);
                    }
                     
                }
            }
        }
        
    }
}