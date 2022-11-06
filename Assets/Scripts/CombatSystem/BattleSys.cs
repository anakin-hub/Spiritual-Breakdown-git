using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, ESCAPE }

public class BattleSys : MonoBehaviour
{
    public List<GameObject> playerPrefab;
    public List<GameObject> enemyPrefab;

    protected Player[] _players;
    protected int MAXplayers;//max player's characters
    protected int playersALIVE;//player's characters alive
    protected int playerACTING;//player character that has the turn happening
    protected Enemy[] _enemys;
    protected int MAXenemys;
    protected int enemysALIVE;
    protected int enemyACTING;

    Character Target;

    protected Character PlayerCharacters;
    protected Character EnemyCharacters;

    public GameObject AttackButton;
    public GameObject HealButton;
    public GameObject SkillButton;
    public GameObject EscapeButton;

    private bool skillUsed;
    private bool healUsed;

    public BattleState state;

    public TMP_Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public GameObject _victoryScene;
    public GameObject _defeatScene;
    public GameObject _escapeScene;

    protected bool endgame;

    int turnPlayer = 0;
    int turnEnemy = 0;

    void Start()
    {
        healUsed = skillUsed = endgame = false;
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    protected void Update()
    {
        if (Target == null)
            if (Input.GetMouseButtonDown(0))
                FindTarget(_enemys);

        if (endgame)
            if (Input.anyKeyDown)
                SceneManager.LoadScene(0);
    }

    void NextTurn()
    {
        Target = null;

        if (state == BattleState.PLAYERTURN)
        {

            playerACTING++;

            if (playerACTING < MAXplayers)
            {
                if (_players[playerACTING].state == TURNSTATE.WAITING)
                {
                    _players[playerACTING].state = TURNSTATE.ACTION;

                    if (state == BattleState.ENEMYTURN)
                    {
                        Debug.Log("ENEMIGO " + playerACTING);
                    }
                    PlayerTurn();
                }
                else if (_players[playerACTING].state == TURNSTATE.DEAD)
                {

                    NextTurn();
                }
            }
            else
            {
                state = BattleState.ENEMYTURN;
                playerACTING = -1;
                NextTurn();
            }

        }
        else if (state == BattleState.ENEMYTURN)
        {

            enemyACTING++;
            if (enemyACTING < MAXenemys)
            {
                if (_enemys[enemyACTING].state == TURNSTATE.WAITING)
                {
                    _enemys[enemyACTING].state = TURNSTATE.ACTION;

                    StartCoroutine(EnemyTurn());
                }
                else if (_enemys[enemyACTING].state == TURNSTATE.DEAD)
                {

                    NextTurn();
                }
            }
            else
            {

                state = BattleState.PLAYERTURN;
                enemyACTING = -1;
                NextTurn();
            }
        }
    }

    IEnumerator SetupBattle()
    {
        MAXplayers = playersALIVE = playerPrefab.Count;
        playerACTING = 0;
        _players = new Player[MAXplayers];
        for (int i = 0; i < MAXplayers; i++)
            _players[i] = playerPrefab[i].GetComponent<Player>();


        MAXenemys = enemysALIVE = enemyPrefab.Count;
        enemyACTING = -1;
        _enemys = new Enemy[MAXenemys];
        for (int i = 0; i < MAXenemys; i++)
            _enemys[i] = enemyPrefab[i].GetComponent<Enemy>();

        Target = null;

        //FALTA CONFIGURAR O UI INICIAL
        PlayerCharacters = playerPrefab[0].GetComponent<Player>();

        EnemyCharacters = enemyPrefab[0].GetComponent<Enemy>();


        if (enemyPrefab.Count > 1)
        {
            dialogueText.text = "Um Grupo de Inimigos aparece...";
        }
        else
        {
            dialogueText.text = "Um " + EnemyCharacters.UnitName + " selvagem aparece...";
        }

        playerHUD.SetHUD(PlayerCharacters);
        enemyHUD.SetHUD(EnemyCharacters);

        yield return new WaitForSeconds(2f);

        _players[playerACTING].state = TURNSTATE.ACTION;
        state = BattleState.PLAYERTURN;
        SkillButton.SetActive(true);
        HealButton.SetActive(true);
        PlayerTurn();
    }

    bool SelectingTarget()
    {
        if (Target == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void DisableTargets(Character[] c)
    {
        for (int i = 0; i < c.Length; i++)
            c[i].SetTarget(false);
    }

    void FindTarget(Character[] c)
    {
        for (int i = 0; i < c.Length; i++)
            if (c[i].GetTarget())
                Target = c[i];

        Debug.Log("ALVO SELECIONADO");
        DisableTargets(c);
        Debug.Log("ALVOS DESABILITADOS");
    }

    void PlayerTurn()
    {
        if (state == BattleState.PLAYERTURN)
        {
            enableButtons();
            dialogueText.text = "Escolha sua ação: ";
        }
        if (state == BattleState.ENEMYTURN)
            NextTurn();
    }

    void disableButtons()
    {
        AttackButton.SetActive(false);
        EscapeButton.SetActive(false);
        if(!skillUsed)
            SkillButton.SetActive(false);
        if(!healUsed)
            HealButton.SetActive(false);
    }

    void enableButtons()
    {
        AttackButton.SetActive(true);
        EscapeButton.SetActive(true);
        if (!skillUsed)
            SkillButton.SetActive(true);
        if (!healUsed)
            HealButton.SetActive(true);
    }

    bool VerifyAttack(int bonusatk, int Def)
    {
        if (state == BattleState.PLAYERTURN)
            dialogueText.text = "Seu ataque: " + bonusatk + "\n Defesa do alvo: " + Def;
        else if (state == BattleState.ENEMYTURN)
            dialogueText.text = "Ataque do Inimigo: " + bonusatk + "\n Sua defesa: " + Def;

        if (bonusatk > Def)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool CheckDeathTeam()
    {
        if (state == BattleState.PLAYERTURN)
        {
            enemysALIVE--;
            if (enemysALIVE <= 0)
            {
                state = BattleState.WON;
                return true;
            }
            else
                return false;
        }
        else if (state == BattleState.ENEMYTURN)
        {
            playersALIVE--;
            if (playersALIVE <= 0)
            {
                state = BattleState.LOST;
                return true;
            }
            else
                return false;
        }
        else { return false; }
    }

    IEnumerator PlayerATK()
    {
        bool isDead = false;

        if (_players[playerACTING].state != TURNSTATE.DEAD)
        {
            bool success = VerifyAttack(_players[playerACTING].GetAtk(), Target.GetDefesa());


            yield return new WaitForSeconds(2.5f);
            if (success)
            {
                int dmg = _players[playerACTING].GetDano();

                if (_players[playerACTING].Critic())
                {
                    dialogueText.text = "Você acertou o inimigo em um ponto crítico!\n";
                    dmg += _players[playerACTING].GetDano();
                }
                else
                {
                    dialogueText.text = "O ataque foi um sucesso!\n";
                }

                yield return new WaitForSeconds(2f);

                isDead = Target.TakeDamage(dmg);

                enemyHUD.setHP(Target.GetCurrentHP());
                dialogueText.text += "Você deu " + dmg + " de dano!";
            }
            else
            {
                dialogueText.text = "O ataque falhou!";
            }

            yield return new WaitForSeconds(2f);
        }

        if (isDead)
        {
            Target.state = TURNSTATE.DEAD;
            if (CheckDeathTeam())
                EndBattle();
        }
        else
        {
            if (_players[playerACTING].state == TURNSTATE.ACTION)
                _players[playerACTING].state = TURNSTATE.WAITING;
        }

        if (playerACTING + 1 >= MAXplayers)
        {

            state = BattleState.ENEMYTURN;
            playerACTING = -1;
        }
        NextTurn();
    }

    IEnumerator PlayerAttack()
    {

        bool success = VerifyAttack(PlayerCharacters.GetAtk(), EnemyCharacters.GetDefesa());
        bool isDead = false;

        yield return new WaitForSeconds(2.5f);
        if (success)
        {
            int dmg = PlayerCharacters.GetDano();

            if (PlayerCharacters.Critic())
            {
                dialogueText.text = "Você acertou o inimigo em um ponto crítico!\n";
                dmg += PlayerCharacters.GetDano();
            }
            else
            {
                dialogueText.text = "O ataque foi um sucesso!\n";
            }

            yield return new WaitForSeconds(2f);

            isDead = EnemyCharacters.TakeDamage(dmg);

            enemyHUD.setHP(EnemyCharacters.GetCurrentHP());
            dialogueText.text += "Você deu " + dmg + " de dano!";
        }
        else
        {
            dialogueText.text = "O ataque falhou!";
        }

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {

        bool isDead = false;
        if (_enemys[enemyACTING].state != TURNSTATE.DEAD)
        {
            int n = _enemys[enemyACTING].DecideMenace(_players[0].GetMenace(), 0, 0, 0);

            Target = _players[n];

            Debug.Log("Target = " + n);

            bool success = VerifyAttack(_enemys[enemyACTING].GetAtk(), Target.GetDefesa());

            yield return new WaitForSeconds(2.5f);

            if (success)
            {
                int dmg = _enemys[enemyACTING].GetDano();

                if (_enemys[enemyACTING].Critic())
                {
                    dialogueText.text = "Você recebeu um golpe em um ponto vital!\n";
                    dmg += _enemys[enemyACTING].GetDano();
                }
                else
                {
                    dialogueText.text = _enemys[enemyACTING].UnitName + " atacou!\n";
                }

                yield return new WaitForSeconds(2f);

                isDead = Target.TakeDamage(dmg);

                playerHUD.setHP(Target.GetCurrentHP());
                dialogueText.text += "Você recebeu " + dmg + " de dano";
            }
            else
            {
                dialogueText.text = "O ataque falhou!";
            }



            yield return new WaitForSeconds(2f);
        }

        if (isDead)
        {
            if (Target.state != TURNSTATE.DEAD)
            {
                Target.state = TURNSTATE.DEAD;
                if (CheckDeathTeam())
                    EndBattle();
                else
                    NextTurn();
            }
        }
        else
        {
            if (_enemys[enemyACTING].state == TURNSTATE.ACTION)
                _enemys[enemyACTING].state = TURNSTATE.WAITING;
        }

        if (enemyACTING + 1 >= MAXenemys)
        {
            state = BattleState.PLAYERTURN;
            enemyACTING = -1;
        }

        NextTurn();
    }

    void EndBattle()
    {
        Target = null;
        GameObject finalscene;

        if (state == BattleState.WON)
        {
            dialogueText.text = "Você venceu o combate!";
            finalscene = _victoryScene;
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "Você perdeu...";
            finalscene = _defeatScene;
        }
        else
        {
            dialogueText.text = "Você fugiu do combate.";
            finalscene = _escapeScene;
        }

        StartCoroutine(Finishing_Game(finalscene));
    }

    IEnumerator Finishing_Game(GameObject finalscene)
    {
        yield return new WaitForSeconds(3f);

        finalscene.SetActive(true);

        yield return new WaitForSeconds(2f);
        endgame = true;
    }

    public void OnAttackButton()
    {
        if (state == BattleState.PLAYERTURN)
        {
            disableButtons();
            StartCoroutine(SetupAttack(PlayerATK()));
        }
    }

    IEnumerator SetupAttack(IEnumerator Skill)
    {
        dialogueText.text = "Selecione seu alvo";

        yield return new WaitUntil(SelectingTarget);//MUDAR PARA SELEÇÃO DE ALVO INIMIGO

        StartCoroutine(Skill);
    }

    IEnumerator PlayerHeal()
    {
        _players[playerACTING].Heal(10);

        playerHUD.setHP(_players[playerACTING].GetCurrentHP());
        dialogueText.text = "Você sente sua vitalidade aumentando";

        yield return new WaitForSeconds(2f);

        if (_players[playerACTING].state == TURNSTATE.ACTION)
            _players[playerACTING].state = TURNSTATE.WAITING;

        state = BattleState.ENEMYTURN;
        playerACTING = -1;

        NextTurn();
    }

    public void OnHealButton()
    {
        if (state == BattleState.PLAYERTURN)
        {
            disableButtons();
            healUsed = true;
            StartCoroutine(PlayerHeal());
        }
    }

    public void OnSkillButton()
    {
        if (state == BattleState.PLAYERTURN)
        {
            disableButtons();
            skillUsed = true;
            StartCoroutine(SetupAttack(FireBall()));
        }
    }

    IEnumerator FireBall()
    {
        bool isDead = false;

        if (_players[playerACTING].state != TURNSTATE.DEAD)
        {
            bool success = VerifyAttack(_players[playerACTING].GetAtk() + 3, Target.GetDefesa());


            yield return new WaitForSeconds(2.5f);
            if (success)
            {
                int dmg = _players[playerACTING].GetDano();
                dmg += _players[playerACTING].GetDano();
                dmg += _players[playerACTING].GetDano();

                dialogueText.text = "A habilidade acertou!\n";
                               
                yield return new WaitForSeconds(2f);

                isDead = Target.TakeDamage(dmg);

                enemyHUD.setHP(Target.GetCurrentHP());
                dialogueText.text += "Você deu " + dmg + " de dano!";
            }
            else
            {
                dialogueText.text = "O ataque falhou!";
            }

            yield return new WaitForSeconds(2f);
        }

        if (isDead)
        {
            Target.state = TURNSTATE.DEAD;
            if (CheckDeathTeam())
                EndBattle();
        }
        else
        {
            if (_players[playerACTING].state == TURNSTATE.ACTION)
                _players[playerACTING].state = TURNSTATE.WAITING;
        }

        if (playerACTING + 1 >= MAXplayers)
        {

            state = BattleState.ENEMYTURN;
            playerACTING = -1;
        }
        NextTurn();
    }

    public void OnEscapeButton()
    {
        if (state == BattleState.PLAYERTURN)
        {
            disableButtons();
            StartCoroutine(EscapeCombat());
        }
    }

    IEnumerator EscapeCombat()
    {
        dialogueText.text = "Você tenta fugir do combate.";

        yield return new WaitForSeconds(2f);

        if(_players[playerACTING].RollDice(100) > 5)
        {
            dialogueText.text = "Você escapou do combate.";
            EndBattle();
            state = BattleState.WON;
        }
        else
        {
            dialogueText.text = "Você não conseguiu escapar e o inimigo parece estar pronto para te atacar.";
            if (_players[playerACTING].state == TURNSTATE.ACTION)
                _players[playerACTING].state = TURNSTATE.WAITING;
            state = BattleState.ENEMYTURN;
            playerACTING = -1;
            NextTurn();
        }
    }
}
