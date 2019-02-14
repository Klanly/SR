using UnityEngine;
using System.Collections;

public interface BattleUIInterface {

	void OnRegisterSWFCallback(NGBattleUIScript obj);
	void usePotionInBattle(bool used);
	void onRatingSound();
	void StaffCharged();
	void SpiritCharged();
	void onRuneSpellTutorialStart();
	void onRuneSpellTutorial2();
	void onRuneSpellTutorialEnd();
	void onSpellBurstTutorialStart();
	void onSpellBurstTutorialEnd();
	void onSpiritFightTutorial2();
	void onSpiritFightTutorialEnd();
	void onSpiritFightTutorialEnding();
	
}
