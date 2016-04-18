using UnityEngine;
using System.Collections;

public class EnemyMelee : BasicObject {

	public LayerMask layerMask;
	private bool impacted = false;
	private float skinWidth  = 0.1f; //probably doesn't need to be changed 
	private float minimumExtent; 
	private float partialExtent; 
	private float sqrMinimumExtent; 
	private Vector3 previousPosition; 
	private Rigidbody myRigidbody;

	private Vector3 lastPosition;

	void Awake() { 
		myRigidbody = GetComponent<Rigidbody>();
		previousPosition = myRigidbody.position; 
		minimumExtent = Mathf.Min(Mathf.Min(GetComponent<Collider>().bounds.extents.x, GetComponent<Collider>().bounds.extents.y), GetComponent<Collider>().bounds.extents.z); 
		partialExtent = minimumExtent * (1.0f - skinWidth); 
		sqrMinimumExtent = minimumExtent * minimumExtent; 

	}


	void FixedUpdate() {


		Vector3 direction = transform.position - lastPosition;
		Ray ray = new Ray(lastPosition, direction);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray, out hit, direction.magnitude, layerMask))    {
			Impact(hit.point, hit.normal, hit.collider.gameObject);
		}

		this.lastPosition = transform.position;
	}

	void OnCollisionEnter(Collision collision) {

		if(impacted == true) {
			return;
		}
		Impact(collision.contacts[0].point, collision.contacts[0].normal, collision.collider.gameObject);
		impacted = true;
	}

	void Impact(Vector3 pos, Vector3 normal, GameObject hitObject) {
		//Apply damage to the hit gameObject, or whatever
		Destroy(gameObject);
	}
		
}

 u s i n g   S y s t e m . C o l l e c t i o n s ; 
 
 p u b l i c   c l a s s   E n e m y S h o o t i n g   :   B a s i c O b j e c t   { 
 
         / /   T h e   m a x i m u m   p o t e n t i a l   d a m a g e   p e r   s h o t . 
         p u b l i c   f l o a t   m a x i m u m D a m a g e   =   1 2 0 f ; 
         / /   T h e   m i n i m u m   p o t e n t i a l   d a m a g e   p e r   s h o t . 	 	 
         p u b l i c   f l o a t   m i n i m u m D a m a g e   =   4 5 f ; 
         / /   A n   a u d i o   c l i p   t o   p l a y   w h e n   a   s h o t   h a p p e n s . 	 	 
         p u b l i c   A u d i o C l i p   s h o t C l i p ; 
         / /   T h e   i n t e n s i t y   o f   t h e   l i g h t   w h e n   t h e   s h o t   h a p p e n s . 	 	 	 
         p u b l i c   f l o a t   f l a s h I n t e n s i t y   =   3 f ; 
         / /   H o w   f a s t   t h e   l i g h t   w i l l   f a d e   a f t e r   t h e   s h o t . 	 	 
         p u b l i c   f l o a t   f a d e S p e e d   =   1 0 f ; 
 
         / /   R e f e r e n c e   t o   t h e   a n i m a t o r . 
         A n i m a t o r   a n i m ; 
         / /   R e f e r e n c e   t o   t h e   H a s h I D s   s c r i p t . 	 	 	 	 	 
         H a s h I D s   h a s h ; 
         / /   R e f e r e n c e   t o   t h e   l a s e r   s h o t   l i n e   r e n d e r e r . 	 	 	 	 
         L i n e R e n d e r e r   l a s e r S h o t L i n e ; 
         / /   R e f e r e n c e   t o   t h e   l a s e r   s h o t   l i g h t . 	 
         L i g h t   l a s e r S h o t L i g h t ; 
         / /   R e f e r e n c e   t o   t h e   s p h e r e   c o l l i d e r . 	 	 	 	 
         S p h e r e C o l l i d e r   c o l ; 
   
         / /   A   b o o l   t o   s a y   w h e t h e r   o r   n o t   t h e   e n e m y   i s   c u r r e n t l y   s h o o t i n g . 
         b o o l   s h o o t i n g ; 
         / /   A m o u n t   o f   d a m a g e   t h a t   i s   s c a l e d   b y   t h e   d i s t a n c e   f r o m   t h e   p l a y e r . 	 	 	 	 
         f l o a t   s c a l e d D a m a g e ; 	 	 	 	 	 	 	 
 
         v o i d   A w a k e ( )   { 
                 / /   S e t t i n g   u p   t h e   r e f e r e n c e s . 
                 a n i m   =   G e t C o m p o n e n t < A n i m a t o r > ( ) ; 
                 l a s e r S h o t L i n e   =   G e t C o m p o n e n t I n C h i l d r e n < L i n e R e n d e r e r > ( ) ; 
                 l a s e r S h o t L i g h t   =   l a s e r S h o t L i n e . g a m e O b j e c t . G e t C o m p o n e n t < L i g h t > ( ) ; 
                 c o l   =   G e t C o m p o n e n t < S p h e r e C o l l i d e r > ( ) ; 
                 h a s h   =   G a m e O b j e c t . F i n d G a m e O b j e c t W i t h T a g ( T a g s . g a m e C o n t r o l l e r ) . G e t C o m p o n e n t < H a s h I D s > ( ) ; 
 
                 / /   T h e   l i n e   r e n d e r e r   a n d   l i g h t   a r e   o f f   t o   s t a r t . 
                 l a s e r S h o t L i n e . e n a b l e d   =   f a l s e ; 
                 l a s e r S h o t L i g h t . i n t e n s i t y   =   0 f ; 
 
                 / /   T h e   s c a l e d D a m a g e   i s   t h e   d i f f e r e n c e   b e t w e e n   t h e   m a x i m u m   a n d   t h e   m i n i m u m   d a m a g e . 
                 s c a l e d D a m a g e   =   m a x i m u m D a m a g e   -   m i n i m u m D a m a g e ; 
         } 
 
         v o i d   U p d a t e ( )   { 
                 / /   C a c h e   t h e   c u r r e n t   v a l u e   o f   t h e   s h o t   c u r v e . 
                 f l o a t   s h o t   =   a n i m . G e t F l o a t ( h a s h . s h o t F l o a t ) ; 
 
                 / /   I f   t h e   s h o t   c u r v e   i s   p e a k i n g   a n d   t h e   e n e m y   i s   n o t   c u r r e n t l y   s h o o t i n g . . . 
                 i f   ( s h o t   >   0 . 5 f   & &   ! s h o o t i n g )   { 
                         / /   . . .   s h o o t 
                         S h o o t ( ) ; 
                 } 
 
                 / /   I f   t h e   s h o t   c u r v e   i s   n o   l o n g e r   p e a k i n g . . . 
                 i f   ( s h o t   <   0 . 5 f )   { 
                         / /   . . .   t h e   e n e m y   i s   n o   l o n g e r   s h o o t i n g   a n d   d i s a b l e   t h e   l i n e   r e n d e r e r . 
                         s h o o t i n g   =   f a l s e ; 
                         l a s e r S h o t L i n e . e n a b l e d   =   f a l s e ; 
                 } 
 
                 / /   F a d e   t h e   l i g h t   o u t . 
                 l a s e r S h o t L i g h t . i n t e n s i t y   =   M a t h f . L e r p ( l a s e r S h o t L i g h t . i n t e n s i t y ,   0 f ,   f a d e S p e e d   *   T i m e . d e l t a T i m e ) ; 
         } 
 
         v o i d   O n A n i m a t o r I K ( i n t   l a y e r I n d e x )   { 
                 / /   C a c h e   t h e   c u r r e n t   v a l u e   o f   t h e   A i m W e i g h t   c u r v e . 
                 f l o a t   a i m W e i g h t   =   a n i m . G e t F l o a t ( h a s h . a i m W e i g h t F l o a t ) ; 
 
                 / /   S e t   t h e   I K   p o s i t i o n   o f   t h e   r i g h t   h a n d   t o   t h e   p l a y e r ' s   c e n t r e . 
                 a n i m . S e t I K P o s i t i o n ( A v a t a r I K G o a l . R i g h t H a n d ,   p l a y e r R e f e r e n c e . G e t C o m p o n e n t < P l a y e r C o n t r o l > ( ) . h e a d . p o s i t i o n ) ; 
 
                 / /   S e t   t h e   w e i g h t   o f   t h e   I K   c o m p a r e d   t o   a n i m a t i o n   t o   t h a t   o f   t h e   c u r v e . 
                 a n i m . S e t I K P o s i t i o n W e i g h t ( A v a t a r I K G o a l . R i g h t H a n d ,   a i m W e i g h t ) ; 
         } 
 
         v o i d   S h o o t ( )   { 
                 / /   T h e   e n e m y   i s   s h o o t i n g . 
                 s h o o t i n g   =   t r u e ; 
 
                 / /   T h e   f r a c t i o n a l   d i s t a n c e   f r o m   t h e   p l a y e r ,   1   i s   n e x t   t o   t h e   p l a y e r ,   0   i s   t h e   p l a y e r   i s   a t   t h e   e x t e n t   o f   t h e   s p h e r e   c o l l i d e r . 
                 f l o a t   f r a c t i o n a l D i s t a n c e   =   ( c o l . r a d i u s   -   V e c t o r 3 . D i s t a n c e ( t r a n s f o r m . p o s i t i o n ,   p l a y e r R e f e r e n c e . G e t C o m p o n e n t < P l a y e r C o n t r o l > ( ) . h e a d . p o s i t i o n ) )   /   c o l . r a d i u s ; 
 
                 / /   T h e   d a m a g e   i s   t h e   s c a l e d   d a m a g e ,   s c a l e d   b y   t h e   f r a c t i o n a l   d i s t a n c e ,   p l u s   t h e   m i n i m u m   d a m a g e . 
                 f l o a t   d a m a g e   =   s c a l e d D a m a g e   *   f r a c t i o n a l D i s t a n c e   +   m i n i m u m D a m a g e ; 
 
                 / /   D i s p l a y   t h e   s h o t   e f f e c t s . 
                 S h o t E f f e c t s ( ) ; 
         } 
 
         v o i d   S h o t E f f e c t s ( )   { 
                 / /   S e t   t h e   i n i t i a l   p o s i t i o n   o f   t h e   l i n e   r e n d e r e r   t o   t h e   p o s i t i o n   o f   t h e   m u z z l e . 
                 l a s e r S h o t L i n e . S e t P o s i t i o n ( 0 ,   l a s e r S h o t L i n e . t r a n s f o r m . p o s i t i o n ) ; 
 
                 / /   S e t   t h e   e n d   p o s i t i o n   o f   t h e   p l a y e r ' s   c e n t r e   o f   m a s s . 
                 l a s e r S h o t L i n e . S e t P o s i t i o n ( 1 ,   p l a y e r R e f e r e n c e . G e t C o m p o n e n t < P l a y e r C o n t r o l > ( ) . h e a d . p o s i t i o n ) ; 
 
                 / /   T u r n   o n   t h e   l i n e   r e n d e r e r . 
                 l a s e r S h o t L i n e . e n a b l e d   =   t r u e ; 
 
                 / /   M a k e   t h e   l i g h t   f l a s h . 
                 l a s e r S h o t L i g h t . i n t e n s i t y   =   f l a s h I n t e n s i t y ; 
 
                 / /   P l a y   t h e   g u n   s h o t   c l i p   a t   t h e   p o s i t i o n   o f   t h e   m u z z l e   f l a r e . 
                 A u d i o M a n a g e r . i n s t a n c e . P l a y S o u n d A t P o s i t i o n ( s h o t C l i p ,   l a s e r S h o t L i g h t . t r a n s f o r m . p o s i t i o n ) ; 
         } 
 } 
 