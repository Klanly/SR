using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game;

namespace GuildSystem
{
    public interface ChatListener
    {
        void PopulateChatNode(bool isClan, ChatMessage chatMessage);
    }
}