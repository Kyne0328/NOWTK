using System;
using System.Windows;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace NOWT.Objects;

[INotifyPropertyChanged]
public partial class IgnData
{
    [ObservableProperty]
    private Visibility _trackerDisabled;

    [ObservableProperty]
    private Visibility _trackerEnabled;

    [ObservableProperty]
    private Uri _trackerUri;

    [ObservableProperty]
    private string _username;
}

[INotifyPropertyChanged]
public partial class IdentityData
{
    [ObservableProperty]
    private Uri _image;

    [ObservableProperty]
    private string _name;
}

[INotifyPropertyChanged]
public partial class PlayerUIData
{
    [ObservableProperty]
    private string _backgroundColour;

    [ObservableProperty]
    private string _partyColour;

    [ObservableProperty]
    private Guid _partyUuid;

    [ObservableProperty]
    private Guid _Puuid;
}

[INotifyPropertyChanged]
public partial class SeasonData
{
    [ObservableProperty]
    private Guid _currentSeason;

    [ObservableProperty]
    private Guid _previouspreviouspreviousSeason;

    [ObservableProperty]
    private Guid _previouspreviousSeason;

    [ObservableProperty]
    private Guid _previousSeason;
}

[INotifyPropertyChanged]
public partial class SkinData
{
    [ObservableProperty]
    private Uri _aresImage;

    [ObservableProperty]
    private string _aresName;

    [ObservableProperty]
    private Uri _buckyImage;

    [ObservableProperty]
    private string _buckyName;

    [ObservableProperty]
    private Uri _bulldogImage;

    [ObservableProperty]
    private string _bulldogName;

    [ObservableProperty]
    private Uri _cardImage;

    [ObservableProperty]
    private string _cardName;

    [ObservableProperty]
    private Uri _classicImage;

    [ObservableProperty]
    private string _classicName;

    [ObservableProperty]
    private Uri _frenzyImage;

    [ObservableProperty]
    private string _frenzyName;

    [ObservableProperty]
    private Uri _ghostImage;

    [ObservableProperty]
    private string _ghostName;

    [ObservableProperty]
    private Uri _guardianImage;

    [ObservableProperty]
    private string _guardianName;

    [ObservableProperty]
    private Uri _judgeImage;

    [ObservableProperty]
    private string _judgeName;

    [ObservableProperty]
    private Uri _largeCardImage;

    [ObservableProperty]
    private Uri _marshalImage;

    [ObservableProperty]
    private string _marshalName;

    [ObservableProperty]
    private Uri _outlawImage;

    [ObservableProperty]
    private string _outlawName;

    [ObservableProperty]
    private Uri _meleeImage;

    [ObservableProperty]
    private string _meleeName;

    [ObservableProperty]
    private Uri _odinImage;

    [ObservableProperty]
    private string _odinName;

    [ObservableProperty]
    private Uri _operatorImage;

    [ObservableProperty]
    private string _operatorName;

    [ObservableProperty]
    private Uri _phantomImage;

    [ObservableProperty]
    private string _phantomName;

    [ObservableProperty]
    private Uri _sheriffImage;

    [ObservableProperty]
    private string _sheriffName;

    [ObservableProperty]
    private Uri _shortyImage;

    [ObservableProperty]
    private string _shortyName;

    [ObservableProperty]
    private Uri _spectreImage;

    [ObservableProperty]
    private string _spectreName;

    [ObservableProperty]
    private Uri _spray1Image;

    [ObservableProperty]
    private string _spray1Name;

    [ObservableProperty]
    private Uri _spray2Image;

    [ObservableProperty]
    private string _spray2Name;

    [ObservableProperty]
    private Uri _spray3Image;

    [ObservableProperty]
    private string _spray3Name;

    [ObservableProperty]
    private Uri _spray4Image;

    [ObservableProperty]
    private string _spray4Name;

    [ObservableProperty]
    private Uri _stingerImage;

    [ObservableProperty]
    private string _stingerName;

    [ObservableProperty]
    private Uri _vandalImage;

    [ObservableProperty]
    private string _vandalName;
}

[INotifyPropertyChanged]
public partial class RankData
{
    [ObservableProperty]
    private int _maxRr = 100;

    [ObservableProperty]
    private Uri[] _rankImages;

    [ObservableProperty]
    private string[] _rankNames;
}

[INotifyPropertyChanged]
public partial class MatchHistoryData
{
    [ObservableProperty]
    private int[] _previousGames;

    [ObservableProperty]
    private string[] _previousGameColours;

    [ObservableProperty]
    private int _rankProgress;

    [ObservableProperty]
    private int _matchesPlayedTogether;

    [ObservableProperty]
    private string _lastPlayedTime;
}

public class MatchHistoryEntry
{
    public string MatchId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Agent { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public int Assists { get; set; }
    public bool Won { get; set; }
}

public class ValMap
{
    public string Name { get; set; }
    public Guid UUID { get; set; }
}

public class ValCard
{
    public string Name { get; set; } = "Undefined";
    public Uri Image { get; set; } =
        new Uri(
            "https://media.valorant-api.com/sprays/472693f9-4d87-416b-9def-3fbe2d310cc0/displayicon.png"
        );
    public Uri FullImage { get; set; } =
        new Uri(
            "https://media.valorant-api.com/sprays/472693f9-4d87-416b-9def-3fbe2d310cc0/displayicon.png"
        );
}

public class ValNameImage
{
    public string Name { get; set; } = "Undefined";
    public Uri Image { get; set; } =
        new Uri(
            "https://media.valorant-api.com/sprays/472693f9-4d87-416b-9def-3fbe2d310cc0/displayicon.png"
        );
}

[INotifyPropertyChanged]
public partial class MatchDetails
{
    [ObservableProperty]
    private string _gameMode;

    [ObservableProperty]
    private Uri _gameModeImage;

    [ObservableProperty]
    private string _map;

    [ObservableProperty]
    private Uri _mapImage;
}

[INotifyPropertyChanged]
public partial class Player
{
    [ObservableProperty]
    private string _accountLevel;

    [ObservableProperty]
    private Visibility _active = Visibility.Collapsed;

    [ObservableProperty]
    private IdentityData _identityData;

    [ObservableProperty]
    private IgnData _ignData;

    [ObservableProperty]
    private MatchHistoryData _matchHistoryData;

    [ObservableProperty]
    private PlayerUIData _playerUiData;

    [ObservableProperty]
    private RankData _rankData;

    [ObservableProperty]
    private SkinData _skinData;

    [ObservableProperty]
    private string _teamId;

    [ObservableProperty]
    private bool _isAgentLocked;

    // New properties for enhanced features
    [ObservableProperty]
    private double _headshotPercent;

    [ObservableProperty]
    private double _winRate;

    [ObservableProperty]
    private int _gamesPlayed;

    [ObservableProperty]
    private int _leaderboardPosition;

    [ObservableProperty]
    private string _partyIcon;

    [ObservableProperty]
    private string _previousRankName;

    [ObservableProperty]
    private string _peakRankName;

    [ObservableProperty]
    private int _matchesPlayedTogether;

    [ObservableProperty]
    private string _lastPlayedTime;

    [ObservableProperty]
    private string _playerName;

    [ObservableProperty]
    private string _playerTag;

    [ObservableProperty]
    private string _playerUuid;

    [ObservableProperty]
    private double _killDeathRatio;
}

[INotifyPropertyChanged]
public partial class LoadingOverlay
{
    [ObservableProperty]
    private string _content;

    [ObservableProperty]
    private string _header;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private int _progress;
}

[INotifyPropertyChanged]
public partial class RoundStat
{
    [ObservableProperty]
    private int _roundNumber;

    [ObservableProperty]
    private int _playerKills;

    [ObservableProperty]
    private int _playerDeaths;

    [ObservableProperty]
    private int _opponentKills;

    [ObservableProperty]
    private int _opponentDeaths;
}
