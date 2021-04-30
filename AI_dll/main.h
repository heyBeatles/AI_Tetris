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



//AI�㷨�����ã�����C#�����н���
//  ���������״̬����(15*10)��ש����״����(5*5)
//  ���̣���������ש���������ת��״����Ϊÿ����״����EvaluateShape�����ó�����״����Ѱڷ�λ�ã�
//        ��󷵻�������״����Ѱڷ�λ���л������ֵ��һ��
//  ���أ�һ���з������������ӳ�ʼλ���ƶ�����Ѱڷ�λ������ƽ�Ƶ�ˮƽ���룬
//extern  "C" _declspec(dllexport) int ComputerAIPlayer(int game[15][10],int brick[5][5]);



//��������
//AI�㷨�����ú��������Ǹ��ݴ˺������ҷ���һ��X�᷽����ѵ�ƫ��ֵ������C#����
//���ݴ�X����ƫ��ֵ�����Զ��ƶ�
        //  ��Ѱڷ�λ����������������������ת�÷��飩
        //  ���������״̬����(15*10)������ת��ש�����״����(5*5)
        //  ���̣�������ש��ɰڷŵ�����λ�ã�Ϊÿ���ɰڷ�λ�õ���EvaluateFunction�����ó���λ�õ�����ֵ
        //        �����Ѱڷ�λ�ú͸�λ�õ�����ֵ����������ֵ��ͬ��λ����Ҫ����PrioritySelection������
        //        ��Ѱڷ�λ��
        //  ���أ�һ���з������������ӳ�ʼλ���ƶ�����Ѱڷ�λ������ƽ�Ƶ�ˮƽ���룬
extern  "C" _declspec(dllexport) int EvaluateShape(int game[15][10],int brick[5][5]);




//���ȶ�ѡ����
//�����������������ֵһ��������Ҫ�������ȶȽ���ѡ��
//����row��colΪĳ�ɰڷ�λ�����ڵ��к���
//��EvaluateShape��������
extern  "C" _declspec(dllexport) int PrioritySelection(int game[15][10],int brick[5][5],int row, int col);


//��������
//����6�����Եļ��㷽��ʹ�����������ó�����ֵ
//����row��colΪĳ�ɰڷ�λ�����ڵ��к���
//����������EvaluateShape��������
//��������ú�����������Լ��㺯��
extern  "C" _declspec(dllexport) int EvaluateFunction(int game[15][10],int brick[5][5],int row, int col);


//���ļ��㺯��
//����row��colΪĳ�ɰڷ�λ�����ڵ��к���
extern  "C" _declspec(dllexport) int GetBoardWells(int game[15][10],int brick[5][5],int row, int col);




//���ն����ļ��㺯��
//����row��colΪĳ�ɰڷ�λ�����ڵ��к���
extern  "C" _declspec(dllexport) int GetBoardBuriedHoles(int game[15][10],int brick[5][5],int row, int col);


//boardRowTransitionss�ļ��㺯��
//����row��colΪĳ�ɰڷ�λ�����ڵ��к���
extern  "C" _declspec(dllexport) int GetBoardRowTransitions(int game[15][10],int brick[5][5],int row, int col);



//boardColTransitions�ļ��㺯��
//����row��colΪĳ�ɰڷ�λ�����ڵ��к���
extern  "C" _declspec(dllexport) int GetboardColTransitions(int game[15][10],int brick[5][5],int row, int col);



//erodedPieceCellsMetric�ļ��㺯��
//����row��colΪĳ�ɰڷ�λ�����ڵ��к���
extern  "C" _declspec(dllexport) int GetErodedPieceCellsMetric(int game[15][10],int brick[5][5],int row, int col);


//landingHeight�ļ��㺯��
//����row��colΪĳ�ɰڷ�λ�����ڵ��к���
extern  "C" _declspec(dllexport) int GetLandingHeight(int game[15][10],int brick[5][5],int row, int col);





#endif

#endif // __MAIN_H__
