using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AAShared;

using XMLib;
using XMLib.Extensions;

namespace AAStats
{
    public class Game
    {
        private CharactersManager m_charactersManager;

        private AAOverlay m_overlay;

        private PacketsProcessor m_processor;


        public Game(PacketsProcessor _processor)
        {
            m_processor = _processor;
            m_charactersManager = new CharactersManager(_processor);
            m_charactersManager.BuffUpdated += CharactersManagerOnBuffUpdated;
         
            _processor.PlayerIdChanged += ProcessorOnPlayerIdChanged;
            _processor.TargetChanged += ProcessorOnTargetChanged;
            m_overlay = AAOverlay.Create();
           
        }

        private void CharactersManagerOnBuffUpdated(object _sender, BuffUpdatedEventArgs _buffUpdatedEventArgs)
        {
            var buff = _buffUpdatedEventArgs.Buff;
            if (buff.CharId == m_processor.PlayerId)
            {
                UpdatePlayer();
            }
            if (buff.CharId == m_processor.TargetId)
            {
                UpdateTarget();
            }
        }

        private void ProcessorOnTargetChanged(object _sender, EventArgs<uint> _eventArgs)
        {
            UpdateTarget();
        }

        private void ProcessorOnPlayerIdChanged(object _sender, EventArgs<uint> _eventArgs)
        {
            UpdatePlayer();
            m_overlay.InvokeSafe(() => m_overlay.WarningVisibility = false);
        }

        private void UpdatePlayer()
        {
            var characterId = m_processor.PlayerId;
            var player = m_charactersManager.GetOrCreate(characterId);
            m_overlay.InitializePlayerCharacter(player);
        }

        private void UpdateTarget()
        {
            var targetId = m_processor.TargetId;
            var target = m_charactersManager.GetOrCreate(targetId);
            m_overlay.InitializeTargetCharacter(target);
        }


       
    }
}
