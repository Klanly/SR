using UnityEngine;
using System.Collections;

public class NGRaidBossDetailInfo : MonoBehaviour
{

    public NGRaidBossDetailInfo _bossLockedPanel;
    public UILabel _raidsDurationLabel;
    public UILabel _raidsUnlockTimeLabel;
    public UILabel _bossDetail_bossNameLabel;
    public UILabel _bossDetail_bossTypeLabel;
    public UILabel _locked_bossNameLabel;
    public UILabel _locked_bossTypeLabel;
    public UILabel _bossDescriptionLabel;
    public UILabel _bossHealthPercentage;
    public UILabel _raidersValueLabel;
    public UILabel _rewardsValueLabel;
    public UISlider _bossHealthFill;
    public UIButton _collectButton;
    [HideInInspector]
    public RaidPortalNavigator
        _portalNavigator;

	private IDictionary dictionary = null;


    public void ShowMonsterUI(IDictionary dictionary, bool locked = false)
    {
		this.dictionary = dictionary;
        UIPanel p;

        gameObject.SetActive(false);

        if (_bossLockedPanel != null)
            _bossLockedPanel.gameObject.SetActive(false);

        IDictionary monsterInfo = dictionary ["currentMonsterInfo"] as IDictionary;

        if (monsterInfo == null)
        {
            gameObject.SetActive(false);

            _bossLockedPanel.gameObject.SetActive(false);
            return;
        }

		IDictionary staticDataDictionary = monsterInfo ["staticData"] as IDictionary;
		IDictionary dynamicDataDictionary = monsterInfo ["dynamicData"] as IDictionary;

        _currentBossDictionary = staticDataDictionary;
        if (!bool.Parse(dynamicDataDictionary ["active"].ToString()) && !locked)
        {
            gameObject.SetActive(false);
            _bossLockedPanel.gameObject.SetActive(true);
            _bossLockedPanel.ShowMonsterUI(dictionary, true);
            return;
        }

        gameObject.SetActive(true);

        if (locked)
        { //when boss is unavailable...
            _bossDetail_bossNameLabel.text = staticDataDictionary ["name"].ToString();
            _bossDescriptionLabel.text = staticDataDictionary ["introduction"].ToString(); //remove
			if (!bool.Parse(staticDataDictionary ["enabled"].ToString()))
			{ // show next enable time as the current boss is defeated
				_raidsUnlockTimeLabel.text = "Coming soon!!";
			} else
			{
//				long _time = Helpers.GetTimeDifferenceInSeconds(long.Parse(dynamicDataDictionary ["startTimeTS"].ToString()), GameManager.ServerTime);
				long _time = Helpers.GetTimeDifferenceInSeconds(long.Parse(dynamicDataDictionary ["startTimeTS"].ToString()), RaidsManager.Instance.TimeOffset);
				_raidsUnlockTimeLabel.text = Upgradeui.ConvertTime((int)_time);
				StopCoroutine("StartCountdown");
				StartCoroutine("StartCountdown", ((int)_time));
			}
        } else
        {

            _bossDetail_bossNameLabel.text = staticDataDictionary ["name"].ToString();
            _bossDetail_bossTypeLabel.text = staticDataDictionary ["title"].ToString(); //remove
            if (bool.Parse(dynamicDataDictionary ["canLoot"].ToString()))
            {
                _bossDescriptionLabel.gameObject.SetActive(false);
                _collectButton.gameObject.SetActive(true);
            } else
            {
                _bossDescriptionLabel.gameObject.SetActive(true);
                _collectButton.gameObject.SetActive(false);
            }
            _bossDescriptionLabel.text = staticDataDictionary ["introduction"].ToString();
            float percentText = (float.Parse(dynamicDataDictionary ["currentLife"].ToString()) / float.Parse(staticDataDictionary ["maxLife"].ToString()));
            _bossHealthPercentage.text = ((int)(percentText * 100)) + " %";
            Debug.LogError("HealthBar: " + (percentText * 100) + "   Id: " + staticDataDictionary ["portalId"].ToString());
            
            _bossHealthFill.value = percentText;
            _raidersValueLabel.text = dynamicDataDictionary ["raiderCount"].ToString();

            IDictionary rewardDictionary = staticDataDictionary ["reward"] as IDictionary; //missing
            _rewardsValueLabel.text = rewardDictionary ["gems"].ToString(); //missing
                        
//			long _time = Helpers.GetTimeDifferenceInSeconds(long.Parse(dynamicDataDictionary ["endTimeTS"].ToString()), GameManager.ServerTime);
			long _time = Helpers.GetTimeDifferenceInSeconds(long.Parse(dynamicDataDictionary ["endTimeTS"].ToString()), RaidsManager.Instance.TimeOffset);
			if (_time < 0)
            { // value is negative
                _raidsDurationLabel.text = Upgradeui.ConvertTime((int)_time * -1) + " ago";
			} else
            {
                _raidsDurationLabel.text = Upgradeui.ConvertTime((int)_time);
				StopCoroutine("StartCountdown");
				StartCoroutine("StartCountdown", ((int)_time));
			}
        }
    }

	IEnumerator StartCountdown(int time) {

		while(time >= 0) {
			if(_raidsDurationLabel != null)
				_raidsDurationLabel.text = Upgradeui.ConvertTime (time--);
			if(_raidsUnlockTimeLabel != null)
				_raidsUnlockTimeLabel.text = Upgradeui.ConvertTime (time--);

			yield return new WaitForSeconds(1.0f);
		}
		if(dictionary == null)
			yield break;

		IDictionary monsterInfo = dictionary ["currentMonsterInfo"] as IDictionary;
		
		if (monsterInfo == null)
		{
			gameObject.SetActive(false);
			
			_bossLockedPanel.gameObject.SetActive(false);
			yield break;
		}
		GameManager.instance.scaleformCamera.generalSwf.ShowUILoadingScreen(true);
		RaidsManager.Instance.GetAllRaidBosses(_portalNavigator.OnRaidBossesLoaded);
		yield return null;
	}
	
	
	
	public void OnCollectButtonClick()
    {
		_collectButton.gameObject.SetActive(false);
        _portalNavigator.OnCollectButton(_currentBossDictionary ["portalId"].ToString());
    }

    private IDictionary _currentBossDictionary;
}
