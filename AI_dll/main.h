#ifndef __MAIN_H__
#define __MAIN_H__

#include <windows.h>

/*  To use this exported function of dll, include this header
 *  in your project.
 */

#ifdef BUILD_DLL
    #define DLL_EXPORT __declspec(dllexport)
#else
    #define DLL_EXPORT __declspec(dllimport)
#endif


#ifdef __cplusplus
extern "C"
{
#endif

void DLL_EXPORT SomeFunction(const LPCSTR sometext);

#ifdef __cplusplus
}

//struct testS
//{
//    int a[15][10];
////    int a;
//    double b;
//};



//AI算法主调用，改由C#代码中进行
//  参数：面板状态数组(15*10)和砖块形状数组(5*5)
//  过程：遍历给定砖块的所有旋转形状，并为每个形状调用EvaluateShape函数得出该形状的最佳摆放位置，
//        最后返回所有形状的最佳摆放位置中获得最优值的一个
//  返回：一个有符号整数，即从初始位置移动到最佳摆放位置所需平移的水平距离，
//extern  "C" _declspec(dllexport) int ComputerAIPlayer(int game[15][10],int brick[5][5]);



//！！！！
//AI算法主调用函数，你们根据此函数给我返回一个X轴方向最佳的偏移值，我在C#代码
//根据此X方向偏移值进行自动移动
        //  最佳摆放位置评估函数（给定已已旋转好方块）
        //  参数：面板状态数组(15*10)和已旋转好砖块的形状数组(5*5)
        //  过程：遍历该砖块可摆放的所有位置，为每个可摆放位置调用EvaluateFunction函数得出该位置的评估值
        //        最后最佳摆放位置和改位置的评估值，遇到评估值相同的位置需要调用PrioritySelection来决定
        //        最佳摆放位置
        //  返回：一个有符号整数，即从初始位置移动到最佳摆放位置所需平移的水平距离，
extern  "C" _declspec(dllexport) int EvaluateShape(int game[15][10],int brick[5][5]);




//优先度选择函数
//假如两个局面的评估值一样，就需要按照优先度进行选择
//参数row、col为某可摆放位置所在的行和列
//被EvaluateShape函数调用
extern  "C" _declspec(dllexport) int PrioritySelection(int game[15][10],int brick[5][5],int row, int col);


//评估函数
//有了6个属性的计算方法使用评估函数得出评估值
//参数row、col为某可摆放位置所在的行和列
//评估函数被EvaluateShape函数调用
//评估会调用后面的六个属性计算函数
extern  "C" _declspec(dllexport) int EvaluateFunction(int game[15][10],int brick[5][5],int row, int col);


//井的计算函数
//参数row、col为某可摆放位置所在的行和列
extern  "C" _declspec(dllexport) int GetBoardWells(int game[15][10],int brick[5][5],int row, int col);




//“空洞”的计算函数
//参数row、col为某可摆放位置所在的行和列
extern  "C" _declspec(dllexport) int GetBoardBuriedHoles(int game[15][10],int brick[5][5],int row, int col);


//boardRowTransitionss的计算函数
//参数row、col为某可摆放位置所在的行和列
extern  "C" _declspec(dllexport) int GetBoardRowTransitions(int game[15][10],int brick[5][5],int row, int col);



//boardColTransitions的计算函数
//参数row、col为某可摆放位置所在的行和列
extern  "C" _declspec(dllexport) int GetboardColTransitions(int game[15][10],int brick[5][5],int row, int col);



//erodedPieceCellsMetric的计算函数
//参数row、col为某可摆放位置所在的行和列
extern  "C" _declspec(dllexport) int GetErodedPieceCellsMetric(int game[15][10],int brick[5][5],int row, int col);


//landingHeight的计算函数
//参数row、col为某可摆放位置所在的行和列
extern  "C" _declspec(dllexport) int GetLandingHeight(int game[15][10],int brick[5][5],int row, int col);





#endif

#endif // __MAIN_H__
