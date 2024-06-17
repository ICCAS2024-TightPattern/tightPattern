using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using OpenAI_API;
using OpenAI_API.Chat;
using System;
using OpenAI_API.Models;

public class gpt : MonoBehaviour
{
    public TMP_Text textField;

    private OpenAIAPI api;
    private List<ChatMessage> messages;

    void Start()
    {
        api = new OpenAIAPI("api 키");
        StartConversation();
        GetResponse();
    }

    private void StartConversation()
    {
        messages = new List<ChatMessage>
        {
            new ChatMessage(ChatMessageRole.System, "당신은 난독증 치료의 전문가 입니다. 놀이공원을 체험한 것을 주제로하여 난독증 치료 읽기 연습에 쓰일 문장 하나를 작문해주세요. 일부러 비슷한 단어로 틀리게 작문해도 됩니다. 형식은 따로 없이, 문장만 출력해주세요") 
        };

        textField.text = ""; // 텍스트 필드 초기화
    }

    private async void GetResponse()
    {
        // 대화 요청을 OpenAI 서버로 전송하여 응답 받음
        var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo,
            Temperature = 0.1,
            MaxTokens = 200,
            Messages = messages
        });

        // 응답 가져오기
        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.Content = chatResult.Choices[0].Message.Content;
        Debug.Log(string.Format("{0}: {1}", responseMessage.rawRole, responseMessage.Content));

        // 응답을 메시지 리스트에 추가
        messages.Add(responseMessage);

        // textField를 응답에 따라 업데이트
        textField.text = responseMessage.Content;
    }
}
