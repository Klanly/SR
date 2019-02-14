using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Test : GameStateModule.GameState
	{
		
		
		
		public interface HelloSayer{
			string SayHello();
			void GetUp();
		}
		
	
		
		private abstract class Employee{
			
		}
			
		
		
		private class Professor : Employee, HelloSayer{
			public string SayHello(){
				return "Hello";
			}
			
			public void GetUp(){
				Debug.Log("I got up on two legs!!!");
			}
		}
		

		private class Dog : HelloSayer{
			public string SayHello(){
				return "Hello";
			}
			
			public void GetUp(){
				Debug.Log("I got up on four legs!!!");
			}
		}
		
		
		private class HelloDetector{
			
			private HelloSayer sayer;
			
			public HelloDetector(HelloSayer sayer){
				this.sayer = sayer;
				this.detect();
				this.sayer.GetUp();
			}
			
			
			private void detect(){
				if(sayer.SayHello().Equals("Hello")){
					Debug.Log("SAYER SAID HELLO!");
				}
			}
			
		}
	
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
	}
	
	
	
	
	
}

