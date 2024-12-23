#ifndef TKACHEVA_GROUP_H
#define TKACHEVA_GROUP_H
#include "boost.h"
#include "tkacheva_headman.h"
#include "ClassStudent.h"

using namespace std;

class tkacheva_group
{
private:
    friend class boost::serialization::access;
public:
    tkacheva_group();
    ~tkacheva_group();
    vector <shared_ptr<tkacheva_student>> group;
    template<class Archive>
    void serialize(Archive& ar, const unsigned int version)
    {
        ar& group;
    }
    void load(string& filename);
    void save(string& filename);
    int get_size();
    void delete_student(int index);
    void add_student(shared_ptr<tkacheva_student> s);
    shared_ptr<tkacheva_student> get(int i);
};
#endif