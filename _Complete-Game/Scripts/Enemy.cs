using UnityEngine;
using System.Collections;

namespace Completed
{

	//Enemy inherits from MovingObject, our base class for objects that can move, Player also inherits from this.
	public class Enemy : MovingObject
	{
		public int playerDamage;                            //The amount of food points to subtract from the player when attacking.
        public int score = 100;
        public AudioClip attackSound1;						//First of two audio clips to play when attacking the player.
		public AudioClip attackSound2;						//Second of two audio clips to play when attacking the player.
        public AudioClip deathSound1;
        public AudioClip deathSound2;
        public float TimeBetweenAttack = 1.0f;

        private Animator animator;							//Variable of type Animator to store a reference to the enemy's Animator component.
		//private Transform target;							//Transform to attempt to move toward each turn.
		//private bool skipMove;								//Boolean to determine whether or not enemy should skip a turn or move this turn.

        private bool needChangeMovingDirection = true;
        private Vector2 movingDirection = Vector2.zero;
        private Vector2[] directions = new Vector2[] { Vector2.up,Vector2.down,Vector2.left,Vector2.right};
        private bool isAttacking;
        bool isDead;

        protected override void Awake()
        {
            base.Awake();

            //Get and store a reference to the attached Animator component.
            animator = GetComponent<Animator>();
            animator.SetTrigger("enemyAppear");
        }

        //Start overrides the virtual Start function of the base class.
        protected override void Start ()
		{
			//Register this enemy with our instance of GameManager by adding it to a list of Enemy objects. 
			//This allows the GameManager to issue movement commands.
			GameManager.instance.AddEnemyToList (this);
			
			//Find the Player GameObject using it's tag and store a reference to its transform component.
			//target = GameObject.FindGameObjectWithTag ("Player").transform;
			
			//Call the start function of our base class MovingObject.
			base.Start ();

        }
       

        //Override the AttemptMove function of MovingObject to include functionality needed for Enemy to skip turns.
        //See comments in MovingObject for more on how base AttemptMove function works.
        protected override void AttemptMove  (int xDir, int yDir)
		{
            //Check if skipMove is true, if so set it to false and skip this turn.
            //if(skipMove)
            //{
            //	skipMove = false;
            //	return;

            //}
            //Call the AttemptMove function from MovingObject.
            base.AttemptMove  (xDir, yDir);
            this.sRender.flipX = xDir > 0;

            //Now that Enemy has moved, set skipMove to true to skip next move.
            //skipMove = true;
        }
		
		
		//MoveEnemy is called by the GameManger each turn to tell each Enemy to try to move towards the player.
		public void MoveEnemy ()
		{
            //Declare variables for X and Y axis move directions, these range from -1 to 1.
            //These values allow us to choose between the cardinal directions: up, down, left and right.
            int yDir = (int)this.movingDirection.y;
            int xDir = (int)this.movingDirection.x;
			
             

			////If the difference in positions is approximately zero (Epsilon) do the following:
			//if(Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon)
				
			//	//If the y coordinate of the target's (player) position is greater than the y coordinate of this enemy's position set y direction 1 (to move up). If not, set it to -1 (to move down).
			//	yDir = target.position.y > transform.position.y ? 1 : -1;
			
			////If the difference in positions is not approximately zero (Epsilon) do the following:
			//else
				////Check if target x position is greater than enemy's x position, if so set x direction to 1 (move right), if not set to -1 (move left).
				//xDir = target.position.x > transform.position.x ? 1 : -1;
			


			//Call the AttemptMove function and pass in the generic parameter Player, because Enemy is moving and expecting to potentially encounter a Player
			AttemptMove  (xDir, yDir);

            if (this.needChangeMovingDirection)
            {
                this.movingDirection = directions[Random.Range(0, directions.Length)];
                this.needChangeMovingDirection = false;
            }
        }
        public void turnTowards(Transform tgt) {

            int xDir = 0,yDir = 0;

            //If the difference in positions is approximately zero (Epsilon) do the following:
            if (Mathf.Abs (tgt.position.x - transform.position.x) < float.Epsilon)

                //If the y coordinate of the target's (player) position is greater than the y coordinate of this enemy's position set y direction 1 (to move up). If not, set it to -1 (to move down).
                yDir = tgt.position.y > transform.position.y ? 1 : -1;

            //If the difference in positions is not approximately zero (Epsilon) do the following:
            else
            //Check if target x position is greater than enemy's x position, if so set x direction to 1 (move right), if not set to -1 (move left).
            xDir = tgt.position.x > transform.position.x ? 1 : -1;

            AttemptMove(xDir, yDir);
        }

        protected override void OnCantMove(Transform transform)
        {
            var player = transform.GetComponent<Player>();
            if (player != null) {
                this.Attack(player);
            } else {
                this.needChangeMovingDirection = true;
            }
        }

        public void Attack(Player player) 
        {
            if (!isAttacking) {
                isAttacking = true;
                var playerHealth = player.GetComponent<PlayerHealth>();
                playerHealth.TakeDamage(playerDamage);
                animator.SetTrigger("enemyAttack");
                SoundManager.instance.RandomizeSfx(attackSound1, attackSound2);

                // reset attacking flag
                this.StartCoroutine(() => isAttacking = false, this.TimeBetweenAttack);
            }
        }
       
        public void Death() {

            if (!isDead)
            {
                isDead = true;
                animator.SetTrigger("enemyDie");
                SoundManager.instance.RandomizeSfx(deathSound1, deathSound2);
                this.rb2D.simulated = false;
                GameManager.instance.RemoveEnemyToList(this);
                ScoreManager.score += this.score;

                Destroy(gameObject, 2f);
            }
        }

        public void Teleport(Vector3 newPosition) {

            isFreezing = true;
            animator.SetTrigger("disappear");

            this.StartCoroutine(() =>
            {
                transform.position = newPosition;
                animator.SetTrigger("enemyAppear");
                this.StartCoroutine(() => isFreezing = false, 2f);
            }, 2f);

        }
    }
}
