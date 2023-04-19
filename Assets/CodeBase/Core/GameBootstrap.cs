using System.Threading;
using CodeBase.AdaptiveDesk;
using CodeBase.AdaptiveDesk._Desk;
using CodeBase.Core._Card;
using CodeBase.Core._Deck;
using CodeBase.Core._Deck.View;
using CodeBase.Core._GameField;
using CodeBase.Core._GameField._GameFieldCase.View;
using CodeBase.Core.DragAndDrop;
using UnityEngine;
using UnityEngine.Serialization;

#pragma warning disable CS4014

namespace CodeBase.Core
{
    public class GameBootstrap : MonoBehaviour
    {
        [Header("Cards")] 
        [SerializeField] private CardsConfiguration _cardsConfiguration;
        [SerializeField] private CardCreator.Settings _cardCreatorSettings;

        [Header("Desk")]
        [SerializeField] private DeskItem[] _deskItems;
        
        [Header("CardDeck")] 
        [SerializeField] private DeckObject _deckObject;
        
        [Header("CardField")] 
        [SerializeField] private GameFieldCaseComponent[] _gameFieldCases;
        [SerializeField] private CardsArranger.Settings _cardsArrangerSettings;
        
        [FormerlySerializedAs("_draggableCards")]
        [Header("Selector")] 
        [SerializeField] private DraggableCardsHolder _draggableCardsHolder;

        

        private CancellationTokenSource _destroyerTokenSource;
        
        private CameraStateObserver _cameraStateObserver;
        private CardCreator _cardCreator;
        private Deck _deck;
        private Desk _desk;
        private Selector _selector;
        private GameFiled _gameFiled;
        private CardsArranger _cardsArranger;
        private CardDropSystem _dropSystem;
        
        private void Start()
            => InitGame();

        private void InitGame()
        {
            _destroyerTokenSource = new CancellationTokenSource();
            
            _cameraStateObserver = new CameraStateObserver(Camera.main);
            _desk = new Desk(_deskItems, _cameraStateObserver);
            _cameraStateObserver.StartObserve(_destroyerTokenSource.Token);

            _cardCreator = new CardCreator(_cardCreatorSettings, _cardsConfiguration);
            _deck = new Deck(_cardCreator.Create(),_deckObject);
            _deck.ReshuffleDeck();

            _cardsArranger = new CardsArranger(_cardsArrangerSettings);
            _gameFiled = new GameFiled(_gameFieldCases, _cardsArranger);
            _gameFiled.FillField(_deck);

            _draggableCardsHolder.InjectArrange(_cardsArranger);
            _dropSystem = new CardDropSystem();
            _selector = new Selector(_gameFiled, _draggableCardsHolder, _dropSystem);
            _selector.StartObserveSelection(_destroyerTokenSource.Token);
        }
        
        private void OnDestroy()
        {
            _destroyerTokenSource.Cancel();
            _desk.Dispose();
        }
    }
}